using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;

/// <summary>
/// 敵の挙動を定義する
/// </summary>
public class EnemyContoroller : MonoBehaviour
{
    [SerializeField]
    private GameObject attackCollider;

    [SerializeField]
    private GameObject m_breath;

    [SerializeField]
    private GameObject m_effect;
    
    [SerializeField]
    private GameObject m_finalBreath;
    
    [SerializeField]
    private Transform m_spwanBreath;
    
    [SerializeField]
    private Transform m_spwanEffect;

    [SerializeField]
    private float m_hitTime = 1f;

    Rigidbody m_rb;
    NavMeshAgent agent;

    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
    }

    public void BasicAttack() => transform.DOMove(transform.position + transform.forward * 2, 0.2f);

    public void BasicAttackEffect()
    {
        var effect = Instantiate(m_effect);
        effect.transform.position = m_spwanEffect.position;
        effect.transform.rotation = Quaternion.Euler(0, 0, -90);
    }

    public void CriticalAttack()
    {
        transform.LookAt(GameManager.Player.transform);
        transform.DOMove(gameObject.transform.position + gameObject.transform.forward * 3, 1f);
    }

    public void JumpAttack()
    {
        agent.SetDestination(GameManager.Player.transform.position);
        agent.speed = 20;
        agent.acceleration = 100;
    }

    public void SetPosition() => agent.SetDestination(transform.position);

    public void JumpAttackEffect()
    {
        StartCoroutine(nameof(WaitNonActive));
    }

    IEnumerator WaitNonActive()
    {
        attackCollider.SetActive(true);
        yield return new WaitForSeconds(m_hitTime);
        attackCollider.SetActive(false);
    }
}
