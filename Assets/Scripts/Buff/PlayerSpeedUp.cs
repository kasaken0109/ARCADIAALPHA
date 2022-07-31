using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class PlayerSpeedUp : IPassiveAction
{
    [SerializeField]
    float _speedUpRate = 1.2f;

    [SerializeField]
    float _effectiveTime = 5f;

    public bool IsEnd { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); 
    }

    public void Execute(int value = 0)
    {
        PassiveActionCaller.Instance.PlayerSpeedUp(_speedUpRate,_effectiveTime);
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
}
