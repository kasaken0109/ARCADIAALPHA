using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BulletReduceEnergy:IPassiveAction
{
    [SerializeField]
    float _reduceRate = 0.6f;

    public bool IsEnd { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public void Execute(int value = 0)
    {
        PassiveActionCaller.Instance.ReduceEnergy(_reduceRate);
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
