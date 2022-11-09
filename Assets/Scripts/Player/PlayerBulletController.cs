using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletController : MonoBehaviour, ICustomSkillEvent
{   
    /// <summary>弾の飛ぶ速度</summary>
    [SerializeField]
    float m_bulletSpeed = 10f;

    [SerializeField]
    GameObject _effect = default;

    [SerializeField]
    GameObject _unHitEffect = default;

    public int Damage { get=> _damage; set => _damage = value; }
    public Action<GameObject> CustomSkillEvent { get => _customSkillEvent; set => _customSkillEvent = value; }
    Action<GameObject> _customSkillEvent;

    private int _damage = 100;

    Rigidbody _rb;

    GameObject call = default;


    void Start()
    {
        TryGetComponent(out _rb);
        _rb.velocity = transform.forward * m_bulletSpeed;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            IDamage idamage;
            other.gameObject.TryGetComponent(out idamage);
            idamage = idamage != null ? idamage : other.gameObject.GetComponentInParent<IDamage>();
            idamage.AddDamage(_damage, ref _effect);
            _customSkillEvent?.Invoke(other.gameObject);
            var pos = other.gameObject.transform.position;
            Instantiate(_effect, pos, Quaternion.identity);
            Destroy(gameObject);
        }
        else
        {
            var pos = other.ClosestPoint(transform.position);
            Instantiate(_unHitEffect, pos, Quaternion.identity);
            Destroy(gameObject, 3);
        }
    }
}
