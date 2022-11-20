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

    [SerializeReference, SubclassSelector]
    ICondition _start = default;

    [SerializeField]
    float _endDuraration = 1f;

    Animator _anim;

    AttackSetController _attackSetController = default;

    PlayerMoveController _playerMove;

    AttackSet[] _currentSets;

    BufferParameter _attackPower;

    ICondition current;

    int playIndex = 0;

    float endTimer = 0;

    bool isPlayFailureState = false;

    void Start()
    {
        TryGetComponent(out _playerMove);
        TryGetComponent(out _attackSetController);
        TryGetComponent(out _anim);
        ChangeAttackSet(_attackNormalSets);
        _attackPower = new BufferParameter(1f);
    }
    void Update()
    {
        var state = current.Check();
        //Debug.Log(current);
        switch (state)
        {
            case ConditionState.Running:
                //Debug.Log("Running");
                break;
            case ConditionState.Success:
                //Debug.Log("Succ");
                _anim.CrossFade(_currentSets[playIndex]._NextStateName,0,0,0,0.3f);
                //_anim.Play(_currentSets[playIndex]._NextStateName);
                current.Reset();
                if(playIndex == _currentSets.Length - 1)
                {
                    playIndex = 0;
                    if (endTimer < _endDuraration)
                    {
                        endTimer += Time.deltaTime;
                    }
                    else
                    {
                        current = _currentSets[playIndex]._condition;
                        current.IsSuccess = false;
                        current.Reset();
                    }
                }
                else
                {
                    playIndex += 1; 
                    current = _currentSets[playIndex]._condition;
                    current.IsSuccess = false;
                    current.Reset();
                }
                break;
            case ConditionState.Failure:
                if (!isPlayFailureState) {
                    _anim.CrossFade(_currentSets[playIndex]._FailedStateName, 0, 0, 0, 0.3f);
                    //_anim.Play(_currentSets[playIndex]._FailedStateName);
                    isPlayFailureState = true;
                }
                current.Reset();
                playIndex = 0;
                CheckPlayerState();
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
        //CheckPlayerState();
        current.IsSuccess = true;
    }

    public void CheckPlayerState()
    {
        isPlayFailureState = false;
        switch (PlayerManager.Instance.PlayerState)
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
    }

    public void ChangeAttackPower(float value, float time)
    {
        StartCoroutine(_attackPower.ChangeValue(value, time));
    }
}
