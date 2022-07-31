using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 攻撃の当たり判定の有効を制御する(主にアニメーションイベントで使用)
/// </summary>
public class AttackGenerator : MonoBehaviour
{
    [SerializeField]
    [Tooltip("頭")]
    private Collider m_head;

    [SerializeField]
    [Tooltip("体")]
    private Collider m_body;

    [SerializeField]
    [Tooltip("爪攻撃の当たり判定")]
    private Collider m_crow;

    [SerializeField]
    [Tooltip("角")]
    private Collider m_horn;

    public void GenerateHeadAttackCollider(float activeTime)
    {
        m_head.gameObject.SetActive(true);
        StartCoroutine(WaitCount(m_head.gameObject, activeTime));
    }

    public void GenerateBodyAttackCollider(float activeTime)
    {
        m_body.gameObject.SetActive(true);
        StartCoroutine(WaitCount(m_body.gameObject, activeTime));
    }

    public void GenerateHornAttackCollider(float activeTime)
    {
        m_horn.gameObject.SetActive(true);
        StartCoroutine(WaitCount(m_horn.gameObject, activeTime));
    }
    public void GenerateCrowAttackCollider(GameObject collider, float activeTime)
    {
        m_crow.gameObject.SetActive(true);
        StartCoroutine(WaitCount(m_crow.gameObject, activeTime));
    }

    IEnumerator WaitCount(GameObject collider, float activeTime)
    {
        yield return new WaitForSeconds(activeTime);
        collider.gameObject.SetActive(false);
        StopCoroutine(WaitCount(collider,activeTime));
    }
}
