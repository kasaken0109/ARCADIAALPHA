using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAttackSpeedup : IPassiveAction
{
    [SerializeField]
    float _attackSpeedUp = 1.2f;

    [SerializeField]
    float _effectiveTime = 5f;
    public bool IsEnd { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public void Execute(float value = 0)
    {
        //PassiveActionCaller.Instance.AttackSpeedUp(_attackSpeedUp, _effectiveTime);
    }
    public void OnAwake()
    {
        throw new System.NotImplementedException();
    }

    public void Execute()
    {
        throw new System.NotImplementedException();
    }

    public void Execute(GameObject set)
    {
        throw new System.NotImplementedException();
    }

    public float GetEffectiveValue()
    {
        throw new System.NotImplementedException();
    }

    public float GetEffectiveTime()
    {
        return 0;
    }
}
