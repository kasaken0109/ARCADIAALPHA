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
    private GameObject _needlePrefab;
    
    [SerializeField]
    private Transform _spwanNeedle;

    [SerializeField]
    private int _needleAttackPower = 50;

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

    public void BasicAttack()
    {
        var target = GameObject.FindGameObjectWithTag("Player");
        if (!target) return;
        var dir = target.transform.position - transform.position;
        dir.y = 0;
        transform.DOMove(transform.position + dir.normalized * 2, 0.4f);
    }

    public void GenerateNeedle()
    {
        var instance = Instantiate(_needlePrefab, _spwanNeedle.position, Quaternion.identity);
        instance.GetComponent<AttackcolliderController>().AttackPower = _needleAttackPower;
    }

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
