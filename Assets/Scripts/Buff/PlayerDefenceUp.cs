using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDefenceUp : IPassiveAction
{
    [SerializeField]
    float _defenceUp = 1.2f;

    [SerializeField]
    float _effectiveTime = 5f;

    float effectiveTime;
    public bool IsEnd { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public void Execute(float value = 0)
    {
        value = value == 0 ? 1 : value;
        effectiveTime = _defenceUp * value;
        Debug.Log($"{effectiveTime}:::::");
        PassiveActionCaller.Instance.PlayerDefenceUp(_defenceUp * value, _effectiveTime);
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
        return _defenceUp;
    }

    public float GetEffectiveTime()
    {
        return _effectiveTime;
    }
}
