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
    [Tooltip("ïêäÌÇÃäÓëbÉ_ÉÅÅ[ÉW")]
    int _weaponDamage = 80;

    Animator _anim;

    AttackSetController _attackSetController = default;

    AttackSet[] _currentSets;

    ICondition current;

    int playIndex = 0;

    void Start()
    {
        TryGetComponent(out _attackSetController);
        TryGetComponent(out _anim);
        ChangeAttackSet(_attackNormalSets);
    }
    void Update()
    {
        if (current.Check() == ConditionState.Success)
        {
            _anim.Play(_currentSets[playIndex]._NextStateName);
            current.Reset();
            playIndex = playIndex == _currentSets.Length - 1 ? 0 : playIndex + 1;
            current = _currentSets[playIndex]._condition;
        }
        else if(current.Check() == ConditionState.Failure)
        {
            current.Reset();
            playIndex = 0;
            current = _currentSets[playIndex]._condition;
        }
    }

    public void SetAttackCollider(AttackData attackData)
    {
        _attackSetController.ActiveAttackCollider(attackData.ActiveColliderIndex, attackData.ActiveDuarration, (int)attackData.AttackRate * _weaponDamage);
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
        _currentSets = set;
        current = set[0]._condition;
    }

    public void AttackSignal()
    {
        CheckPlayerState();
        current.IsSuccess = true;
    }

    void CheckPlayerState()
    {
        switch (PlayerManager.Instance.PlayerState)
        {
            case PlayerState.OnField:
                ChangeAttackSet(_attackNormalSets);
                break;
            case PlayerState.InAir:
                ChangeAttackSet(_attackMidAirSets);
                break;
            case PlayerState.Invisible:
                Debug.Log("ChangeCritical");
                ChangeAttackSet(_attackCriticalSets);
                break;
            default:
                break;
        }
    }
}
