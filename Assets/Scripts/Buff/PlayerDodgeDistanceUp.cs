using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class PlayerDodgeDistanceUp : IPassiveAction
{
    [SerializeField]
    float _dodgeDistanceUp = 1.2f;

    [SerializeField]
    float _effectiveTime = 5f;
    public bool IsEnd { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public void Execute(float value = 0)
    {
        PassiveActionCaller.Instance.DodgeDistanceUp(_dodgeDistanceUp,_effectiveTime);
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
