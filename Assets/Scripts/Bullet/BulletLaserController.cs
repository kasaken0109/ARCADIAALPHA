using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BulletLaserController : MonoBehaviour,ICustomSkillEvent
{
    [SerializeField]
    [Tooltip("射程距離")]
    float _shootRange = 50f;

    [SerializeField]
    [Tooltip("弾の進むスピード")]
    float _speed = 3f;

    [SerializeField]
    [Tooltip("当たるレイヤー")]
    LayerMask _layerMask = 0;

    [SerializeField]
    [Tooltip("命中した時の音")]
    AudioClip _hitSound = null;

    [SerializeField]
    [Tooltip("着弾時に発生するエフェクト")]
    GameObject _effect = null;

    [SerializeField]
    [Tooltip("敵に張り付くエフェクト")]
    GameObject _frostEffect = null;

    [SerializeField]
    [Tooltip("弾の設定")]
    BulletSetting _bulletSetting = default;

    int damage;

    public int Damage { set { damage = value; } }

    public GameObject Target { set => target = value; }

    GameObject target;

    /// <summary>弾着弾時に発生する処理 </summary>
    public Action<GameObject> CustomSkillEvent { get=> _customSkillEvent; set=> _customSkillEvent = value; }

    Action<GameObject> _customSkillEvent;

    const float delayTimeUntilDestroy = 0.5f;
    const float fixVolumeRate = 0.1f;
    bool IsHitSound = false;
    bool EndHit = false;
    Vector3 bulletOrigin;
    Vector3 hitPosition;

    RaycastHit hit;
    Rigidbody _rb;
    GameObject hitObject = null;    // Ray が当たったオブジェクト
    Ray ray;
    
    void Start()
    {
        TryGetComponent(out _rb);
        bulletOrigin = transform.position;
        _rb.velocity = transform.forward * _speed;
        EndHit = false;
    }
    void Update()
    {
        ray = new Ray(transform.position, target.transform.position - transform.position);//Camera.main.ScreenPointToRay(target.transform.position);
        Debug.DrawRay(transform.position, target.transform.position - transform.position, Color.red);
        if (!EndHit)RayHit(ray, ref hitObject);
    }

    /// <summary>
    /// 弾の当たり判定を検知する
    /// </summary>
    /// <param name="ray"></param>
    /// <param name="hitObject">当たったオブジェクト</param>
    /// <returns></returns>
    RaycastHit RayHit(Ray ray, ref GameObject hitObject)
    {
        
        bool IsHit = Physics.Raycast(ray, out hit, _shootRange, _layerMask);

        if (IsHit)
        {
            EndHit = true;
            hitPosition = hit.point;    // Ray が当たった場所
            hitObject = hit.collider.gameObject;    // Ray が洗ったオブジェクト
            Debug.Log(hitObject);

            if (!hitObject) hit = default;
            PlayHitSound(hitPosition);  // レーザーが当たった場所でヒット音を鳴らす
            if (_bulletSetting.HasCriticalDistance) damage = Mathf.CeilToInt((1 - Mathf.Abs(Vector3.Distance(bulletOrigin, hitPosition) - _bulletSetting.CriticalDistance) 
                / _bulletSetting.CriticalDistance * _bulletSetting.ReduceDamagePerDistance) * damage);
            hitObject.GetComponentInParent<IDamage>().AddDamage(damage, ref _effect);
            CustomSkillEvent?.Invoke(hitObject);
            Instantiate(_effect, hitPosition, Quaternion.identity);
            if (_frostEffect)
            {
                Instantiate(_frostEffect, hitPosition, Quaternion.identity, hitObject.transform);
                //SoundManager.Instance.PlayFrost();
            }
            Destroy(this.gameObject, delayTimeUntilDestroy);
        }
        return hit;
    }

    /// <summary>
    /// ヒット音を鳴らす
    /// </summary>
    /// <param name="position">音を鳴らす場所</param>
    void PlayHitSound(Vector3 position)
    {
        if (_hitSound && !IsHitSound)
        {
            AudioSource.PlayClipAtPoint(_hitSound, position, fixVolumeRate);
            IsHitSound = true;
        }
    }
}
