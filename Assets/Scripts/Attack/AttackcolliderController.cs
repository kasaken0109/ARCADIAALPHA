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

    [SerializeField]
    GameObject _attackCallObject = default;

    [SerializeField]
    int id = 0;

    UnityAction<GameObject> _onHit;

    public UnityAction<GameObject> OnHit { set => _onHit = value; }

    public int ID => id; 

    /// <summary>呼ぶオブジェクト</summary>
    GameObject call;
    /// <summary>攻撃が有効かどうか</summary>
    bool canHit;

    public bool CanHit { get => canHit; set => canHit = value; }
    /// <summary>攻撃力</summary>
    int attackPower = 0;

    const int DefaultDamage = 50;

    public int AttackPower {set => attackPower = value; }

    void Start()
    {
        canHit = true;
        call = _attackCallObject;
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
            _onHit?.Invoke(other.gameObject);
            //ダメージを与える
            var idmg = other.gameObject.GetComponentInParent<IDamage>();
            idmg = idmg == null ? other.gameObject.GetComponent<IDamage>() : idmg;
            idmg.AddDamage(attackPower == 0 ? DefaultDamage : attackPower, ref call);

            //ヒット音、エフェクトを再生する
            if (m_hit)SoundManager.Instance.PlayHit(m_hit,gameObject.transform.position);
            if(m_hitEffect) Instantiate(m_hitEffect, other.ClosestPoint(transform.position), m_hitEffect.transform.rotation);

            //ヒット処理を完了したとみなす
            canHit = false;
        }
    }
}
