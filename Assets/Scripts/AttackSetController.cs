using UnityEngine;

/// <summary>
/// 攻撃用のオブジェクトの処理を行う
/// </summary>
public class AttackSetController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("有効にする攻撃のコライダー")]
    GameObject[] _activeCollider;

    [SerializeField]
    [Tooltip("攻撃のコライダーコントローラー")]
    AttackcolliderController[] _attackCollider;
    [SerializeField]
    Animator _anim = default;

    StateController[] _stateController;

    /// <summary>攻撃用のコルーチン</summary>
    Coroutine[] attackColliderCoroutine;

    PlayerMoveController _playerMove;


    void Start()
    {
        attackColliderCoroutine = new Coroutine[_activeCollider.Length];
        _stateController = _anim.GetBehaviours<StateController>();
        TryGetComponent(out _playerMove);
        foreach (var item in _stateController)
        {
            item.SetStateEnterAction(() =>_playerMove.SetMoveActive(false));
            item.SetStateExitAction(() => _playerMove.SetMoveActive(true));
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
        if (colliderIndex < 0 || colliderIndex >= _activeCollider.Length) colliderIndex = 0;
        _attackCollider[colliderIndex].AttackPower = power;
        attackColliderCoroutine[colliderIndex] = null;
        attackColliderCoroutine[colliderIndex] = StartCoroutine(ColliderGenerater.GenerateCollider(_activeCollider[colliderIndex], activeDuraration));
    }
}
