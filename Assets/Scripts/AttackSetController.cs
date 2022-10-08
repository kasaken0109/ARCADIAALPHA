using System.Collections;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 攻撃用のオブジェクトの処理を行う
/// </summary>
public class AttackSetController : MonoBehaviour
{
    [SerializeField]
    List<AttackcolliderController> _attackCollider = new List<AttackcolliderController>();

    [HideInInspector]
    public List<AttackcolliderController> AttackCollider;
    [SerializeField]
    Animator _anim = default;

    StateController[] _stateController;
    JumpAttackStateController[] _jumpAttackStates;

    /// <summary>攻撃用のコルーチン</summary>
    Coroutine[] attackColliderCoroutine;

    PlayerMoveController _playerMove;


    void Start()
    {
        foreach (var item in _attackCollider)
        {
            AttackCollider.Add(item);
        }
        attackColliderCoroutine = new Coroutine[AttackCollider.Count];
        _stateController = _anim.GetBehaviours<StateController>();
        _jumpAttackStates = _anim.GetBehaviours<JumpAttackStateController>();
        TryGetComponent(out _playerMove);
        foreach (var item in _stateController)
        {
            item.SetStateEnterAction(() =>_playerMove.SetMoveActive(false));
            item.SetStateExitAction(() => _playerMove.SetMoveActive(true));
        }
        foreach (var item in _jumpAttackStates)
        {
            item.SetStateEnterAction(() => {
                _playerMove.StartFloat();
                _anim.SetBool("IsAttackEnd", false);
            });
            item.SetStateExitAction(() => {
                _playerMove.SetMoveActive(true);
                _anim.SetBool("IsAttackEnd", true);
            });
        }
    }

    /// <summary>
    /// 剣の当たり判定を有効にする
    /// </summary>
    /// <param name="colliderIndex">有効にするコライダーID</param>
    /// <param name="activeDuraration">有効時間</param>
    /// <param name="power">攻撃力</param>
    public void ActiveAttackCollider(int colliderIndex, float activeDuraration,int power)
    {
        //ID範囲外時の処理
        if (colliderIndex < 0 || colliderIndex >= AttackCollider.Count) colliderIndex = 0;
        AttackCollider[colliderIndex].AttackPower = power;
        attackColliderCoroutine[colliderIndex] = null;
        attackColliderCoroutine[colliderIndex] = StartCoroutine(ColliderGenerater.GenerateCollider(AttackCollider[colliderIndex].gameObject, activeDuraration));
    }
}
