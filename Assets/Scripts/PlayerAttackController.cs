using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    [SerializeReference,SubclassSelector]
    AttackSet[] _attackNormalSets;

    [SerializeReference, SubclassSelector]
    AttackSet[] _attackMidAirSets;

    [SerializeReference, SubclassSelector]
    AttackSet[] _attackCriticalSets;

    [SerializeField]
    float _endDuraration = 1f;

    Animator _anim;

    AttackSetController _attackSetController = default;

    PlayerMoveController _playerMove;

    AttackSet[] _currentSets;

    BufferParameter _attackPower;

    ICondition current;

    PlayerState prev = PlayerState.OnField;

    int playIndex = 0;

    float endTimer = 0;
    void Start()
    {
        TryGetComponent(out _playerMove);
        TryGetComponent(out _attackSetController);
        TryGetComponent(out _anim);
        ChangeAttackSet(_attackNormalSets);
        _attackPower = new BufferParameter(1f);
    }
    void FixedUpdate()
    {
        var state = current.Check();
        StatePlay(state);
    }

    private void StatePlay(ConditionState state)
    {
        switch (state)
        {
            case ConditionState.Running:
                break;
            case ConditionState.Success:
                _playerMove.SetMoveActive(false);
                _anim.CrossFade(_currentSets[playIndex]._NextStateName, 0, 0, 0, 0.3f);
                //_anim.Play(_currentSets[playIndex]._NextStateName);
                current.Reset();
                if (playIndex == _currentSets.Length - 1)
                {
                    playIndex = 0;
                    if (endTimer < _endDuraration)
                    {
                        endTimer += Time.deltaTime;
                    }
                    else
                    {
                        endTimer = 0;
                        _playerMove.SetMoveActive(true);
                        current = _currentSets[playIndex]._condition;
                        current.IsSuccess = false;
                        current.Reset();
                    }
                    return;
                }
                playIndex += 1;
                current = _currentSets[playIndex]._condition;
                current.IsSuccess = false;
                current.Reset();
                break;
            case ConditionState.Failure:
                current.Reset();
                _playerMove.SetMoveActive(true);
                playIndex = 0;
                //CheckPlayerState();
                current = _currentSets[playIndex]._condition;
                break;
        }
    }

    public void SetAttackCollider(AttackData attackData)
    {
        _attackSetController.ActiveAttackCollider(attackData.ActiveColliderIndex, attackData.ActiveDuarration, (int)(attackData.AttackRate * GetComponentInChildren<WeaponAttributeController>().AttackPower * _attackPower.Value));
    }

    void ChangeAttackSet(AttackSet[] set)
    {
        if(_currentSets != null)
        {
            for (int i = 0; i < _currentSets.Length; i++)
            {
                _currentSets[i]._condition.Reset();
            }
        }
        playIndex = 0;
        _currentSets = set;
        current = set[0]._condition;
    }

    public void AttackSignal()
    {
        ChangePlayerState();
        current.IsSuccess = true;
    }

    public void ChangePlayerState()
    {
        var state = PlayerManager.Instance.PlayerState;
        if (state == prev) return;
        switch (state)
        {
            case PlayerState.OnField:
                ChangeAttackSet(_attackNormalSets);
                break;
            case PlayerState.InAir:
                ChangeAttackSet(_attackMidAirSets);
                break;
            case PlayerState.Invisible:
                ChangeAttackSet(_attackCriticalSets);
                break;
            default:
                break;
        }
        prev = state;
    }

    public void ChangeAttackPower(float value, float time)
    {
        StartCoroutine(_attackPower.ChangeValue(value, time));
    }
}
