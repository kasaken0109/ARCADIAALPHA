using System;
using UnityEngine;
using System.Collections.Generic;
using UniRx;

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
    [SerializeField]
    GameObject _animObj = default;

    [SerializeField]
    CameraController _camera = default;

    StateController[] _stateController;
    JumpAttackStateController[] _jumpAttackStates;

    //一時的なもの
    public bool IsPlayer = true;

    /// <summary>攻撃用のコルーチン</summary>
    Coroutine[] attackColliderCoroutine;

    PlayerMoveController _playerMove;

    Transform _target = default;


    void Start()
    {
        foreach (var item in _attackCollider)
        {
            AttackCollider.Add(item);
        }
        attackColliderCoroutine = new Coroutine[AttackCollider.Count];
        if(IsPlayer)AnimActionSet();
        _camera.ObserveEveryValueChanged(x => x.OnLockOnTragetchange).Subscribe(x =>
        {
            if (_camera.IsLockOn)
            {
                _target = x;
            }
            else _target = null;
        }).AddTo(this);
    }

    /// <summary>
    /// アニメーションのステート遷移時のActionを設定する
    /// </summary>
    private void AnimActionSet()
    {
        _stateController = _anim.GetBehaviours<StateController>();
        _jumpAttackStates = _anim.GetBehaviours<JumpAttackStateController>();
        TryGetComponent(out _playerMove);
        foreach (var item in _stateController)
        {
            item.SetStateEnterAction(() => 
            {
                if (_target) _animObj.transform.LookAt(new Vector3(_target.transform.position.x, _animObj.transform.position.y, _target.transform.position.z));
            });
        }
        foreach (var item in _jumpAttackStates)
        {
            item.SetStateEnterAction(() =>
            {
                if (_target) _animObj.transform.LookAt(new Vector3(_target.transform.position.x, _animObj.transform.position.y, _target.transform.position.z));
                _playerMove.StartFloat();
                _anim.SetBool("IsAttackEnd", false);
            });
            item.SetStateExitAction(() =>
            {
                //_playerMove.StopFloat();
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
