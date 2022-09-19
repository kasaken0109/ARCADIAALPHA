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
    [Tooltip("対象との距離")]
    Vector3 m_chaseOffset = default;
    [SerializeField]
    Transform _droneAnim = default;
    [SerializeField]
    [Tooltip("追跡を開始する距離")]
    float _offset = 0.5f;
    [SerializeField]
    [Tooltip("追跡時間")]
    float m_chaseTime = 2f;
    [SerializeField]
    [Tooltip("回転時間")]
    float m_chaseLookTime = 0.5f;
    [SerializeField]
    [Tooltip("浮遊の高さ")]
    float m_floatHeight = 0.7f;
    [SerializeField]
    GameObject[] _boostEffects;


    DroneMode _droneMode = DroneMode.Idle;
    float distance;
    GameObject attackTarget;

    [SerializeField]
    float anglePlus = 50f;

    Rigidbody _rb;
    [SerializeField]
    Animator _anim;
    bool IsEndMove = false;
    bool IsShooting = false;

    void Start()
    {
        attackTarget = GameObject.FindGameObjectWithTag("Enemy");
        TryGetComponent(out _rb);
    }
    void Update()
    {
        LookPlayer();
        //transform.position = new Vector3(transform.position.x, m_chaseTarget.transform.position.y + m_floatHeight, transform.position.z);
        distance = Vector3.Distance(transform.position,m_chaseTarget.transform.position);
        //Debug.Log(distance);
        ChangeMode(IsShooting ? DroneMode.Shooting : distance >= _offset ? DroneMode.Chase : DroneMode.Idle);
        Chase(m_chaseTarget.transform.position);
    }

    /// <summary>
    /// プレイヤーを追いかける
    /// </summary>
    void Chase(Vector3 target)
    {
        var dig = Camera.main.transform.TransformDirection(GameManager.Player.transform.localEulerAngles);
        Vector3 destination = target
            + new Vector3(m_chaseOffset.x * Mathf.Cos(anglePlus / 180f * Mathf.PI),
            0,
            m_chaseOffset.x * Mathf.Sin(anglePlus / 180f * Mathf.PI)
            ) ;
        transform.DOMove(new Vector3(destination.x, 1.5f, destination.z),m_chaseTime);
    }

    /// <summary>
    /// 敵の方向を見る
    /// </summary>
    void LookEnemy()
    {
        if (!attackTarget)
        {
            return;
        }
        Vector3 diff = attackTarget.transform.position - transform.position;
        Quaternion lookAngle = Quaternion.LookRotation(diff);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookAngle, m_chaseLookTime * Time.deltaTime);
    }

    void ChangeMode(DroneMode droneMode)
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
    /// 敵の方向を見る
    /// </summary>
    void LookPlayer()
    {
        
        _droneAnim.rotation = GameManager.Player.transform.rotation;
    }




}
