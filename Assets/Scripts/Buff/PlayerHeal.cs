using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerHeal : IPassiveAction
{
    [SerializeField]
    float _healAmount = 100f;
    public bool IsEnd { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    float healValue = 0;

    public void Execute(float value = 0)
    {
        value = value == 0 ? 1 : value;
        healValue = _healAmount * value;
        PassiveActionCaller.Instance.PlayerHeal(healValue);
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
        return _healAmount;
    }

    public float GetEffectiveTime()
    {
        return 0;
    }
}
