using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 接触判定(攻撃)を制御する
/// </summary>
public class AttackcolliderController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("攻撃対象のタグ名")]
    string m_opponentTagName = "Player";

    [SerializeField]
    [Tooltip("衝突時に発生するエフェクト")]
    GameObject m_hitEffect = null;

    [SerializeField]
    [Tooltip("ヒットサウンド")]
    AudioClip m_hit;

    /// <summary>呼ぶオブジェクト</summary>
    GameObject call;
    /// <summary>攻撃が有効かどうか</summary>
    bool canHit;

    public bool CanHit { get => canHit; set => canHit = value; }
    /// <summary>攻撃力</summary>
    int attackPower = 0;

    public int AttackPower {set => attackPower = value; }

    void Start()
    {
        canHit = true;
        call = gameObject;
    }

    private void OnEnable()
    {
        canHit = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!canHit) return;//連続ヒットを防止する
        if (other.CompareTag(m_opponentTagName))
        {
            //ダメージを与える
            var idmg = other.gameObject.GetComponentInParent<IDamage>();
            idmg = idmg == null ? other.gameObject.GetComponent<IDamage>() : idmg;
            idmg.AddDamage(attackPower, ref call);

            //ヒット音、エフェクトを再生する
            if (m_hit)SoundManager.Instance.PlayHit(m_hit,gameObject.transform.position);
            if(m_hitEffect) Instantiate(m_hitEffect, other.ClosestPoint(transform.position), GameManager.Player.transform.rotation);

            //ヒット処理を完了したとみなす
            canHit = false;
        }
    }
}
