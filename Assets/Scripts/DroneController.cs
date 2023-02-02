using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum DroneMode
{
    Idle,
    Chase,
    Shooting
}
public class DroneController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("追跡対象のオブジェクト")]
    GameObject m_chaseTarget = default;
    [SerializeField]
    [Tooltip("追跡対象のオブジェクト")]
    GameObject m_chaseRotationTarget = default;
    [SerializeField]
    [Tooltip("対象との距離")]
    Vector3 m_chaseOffset = default;
    [SerializeField]
    Transform _droneAnim = default;
    [SerializeField]
    Transform _targetAnim = default;
    [SerializeField]
    [Tooltip("追跡を開始する距離")]
    float _offset = 0.5f;
    [SerializeField]
    [Tooltip("追跡時間")]
    float m_chaseTime = 2f;

    [SerializeField]
    GameObject[] _boostEffects;

    [SerializeField]
    GameObject _movementEffect = default;

    [Header("バフ用の行動フィールド")]

    [SerializeField]
    float _turnoverNum = 10;

    [SerializeField]
    float _bottomHeight = 0.3f;

    [SerializeField]
    float _bottomRadius = 1.5f;

    [SerializeField]
    float _topRadius = 3f;

    [SerializeField]
    float _floatHeightPerLoop = 0.3f;

    [SerializeField]
    float _amplitude = 0.2f;

    float distance;

    [SerializeField]
    float anglePlus = 50f;

    [SerializeField]
    Animator _anim;
    public bool IsShooting = false;
    const float FloatHeight = 1.5f;

    void FixedUpdate()
    {
        if (!IsShooting) LookPlayer();
        //transform.position = new Vector3(transform.position.x, m_chaseTarget.transform.position.y + m_floatHeight, transform.position.z);
        distance = Vector3.Distance(transform.position, m_chaseTarget.transform.position);
        //Debug.Log(distance);
        ChangeMode(IsShooting ? DroneMode.Shooting : distance >= _offset ? DroneMode.Chase : DroneMode.Idle);
        if(!IsShooting) Chase(m_chaseTarget.transform.position);
    }

    /// <summary>
    /// プレイヤーを追いかける
    /// </summary>
    void Chase(Vector3 target)
    {
        Vector3 destination = target
            + new Vector3(m_chaseOffset.x * Mathf.Cos(anglePlus / 180f * Mathf.PI),
            0,
            m_chaseOffset.x * Mathf.Sin(anglePlus / 180f * Mathf.PI)
            ) ;
        transform.DOMove(new Vector3(destination.x, FloatHeight, destination.z),m_chaseTime);
    }

    /// <summary>
    /// 敵の方向を見る
    /// </summary>
    public void LookEnemy(GameObject attackTarget)
    {
        Vector3 diff = attackTarget.transform.position - transform.position;
        Quaternion lookAngle = Quaternion.LookRotation(diff);
        _droneAnim.transform.rotation = Quaternion.LookRotation(diff); //Quaternion.Slerp(_droneAnim.rotation, lookAngle, 0.2f * Time.deltaTime);
    }

    public void LookEnemy(Vector3 attackTarget)
    {
        Vector3 diff = attackTarget - transform.position;
        Quaternion lookAngle = Quaternion.LookRotation(diff);
        _droneAnim.transform.rotation = Quaternion.LookRotation(diff); //Quaternion.Slerp(_droneAnim.rotation, lookAngle, 0.2f * Time.deltaTime);
    }

    public void ChangeMode(DroneMode droneMode)
    {
        switch (droneMode)
        {
            case DroneMode.Idle:
                ChangeBoostEffect(droneMode);
                _anim.SetBool("IsChase", false);
                break;
            case DroneMode.Chase:
                ChangeBoostEffect(droneMode);
                _anim.SetBool("IsChase", true);
                break;
            case DroneMode.Shooting:
                ChangeBoostEffect(droneMode);
                _anim.SetBool("IsChase", true);
                break;
            default:
                break;
        }
    }

    void ChangeBoostEffect(DroneMode droneMode)
    {
        for (int i = 0; i < _boostEffects.Length; i++)
        {
            _boostEffects[i].SetActive(i == (int)droneMode);
        }
    }

    /// <summary>
    /// プレイヤーの方向を見る
    /// </summary>
    void LookPlayer()
    {
        _droneAnim.rotation = _targetAnim.transform.rotation;
    }

    float time = 0;
    Vector3 moveOrigin;
    public void BuffMovement(float moveTime)
    {
        var movementTime = moveTime - 1f < 0 ? 0.5f :moveTime - 1f;
        time = 0;
        moveOrigin = transform.position;
        Vector3 turnStartPos = new Vector3(_bottomRadius * Mathf.Cos(anglePlus / 180f * Mathf.PI), _bottomHeight, _bottomRadius * Mathf.Sin(anglePlus / 180f * Mathf.PI));
        transform.DOMove(turnStartPos + m_chaseTarget.transform.position, 0.5f).OnComplete(() => {
            StartCoroutine(Turnover());
            _movementEffect.SetActive(true);
        });
        IEnumerator Turnover()
        {
            while (time < movementTime)
            {
                var height = _bottomHeight + (_floatHeightPerLoop * _turnoverNum/ movementTime) * time;
                var radius = _bottomRadius + ((_topRadius - _bottomRadius) / movementTime) * time + Mathf.Cos(anglePlus / 180f + time * _turnoverNum / movementTime) * _amplitude;
                transform.position = new Vector3(radius * Mathf.Cos((anglePlus / 180f + time * _turnoverNum / movementTime) * Mathf.PI) + m_chaseTarget.transform.position.x, height, radius * Mathf.Sin((anglePlus / 180f + time * _turnoverNum / movementTime) * Mathf.PI) + m_chaseTarget.transform.position.z);
                _droneAnim.LookAt(GameManager.Player.transform.position + new Vector3(0, height, 0));
                time += Time.deltaTime;
                yield return null;
            }
            transform.DOMove(moveOrigin, 0.5f).OnComplete(() => _movementEffect.SetActive(false));
        }
        
    }




}
