using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackUp : IPassiveAction
{
    [SerializeField]
    float _attackUp = 1.2f;

    [SerializeField]
    float _effectiveTime = 5f;
    public bool IsEnd { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    float effectiveTime;

    public void Execute(float value = 0)
    {
        value = value == 0 ? 1 : value;
        effectiveTime = _attackUp * value;
        PassiveActionCaller.Instance.PlayerAttackUp(_attackUp *value, _effectiveTime);
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
        return _attackUp;
    }

    public float GetEffectiveTime()
    {
        return _effectiveTime;
    }
}
