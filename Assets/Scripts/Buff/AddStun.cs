using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddStun : IPassiveAction
{
    [SerializeField]
    int stunRate = 1;
    public bool IsEnd { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public void Execute(GameObject set)
    {
        set.GetComponentInParent<IStun>().StunChecker(stunRate);
    }

    public void Execute(float value = 0)
    {
        throw new System.NotImplementedException();
    }

    public float GetEffectiveTime()
    {
        return 0;
    }

    public float GetEffectiveValue()
    {
        return 0;
    }

    public void OnAwake()
    {
        throw new System.NotImplementedException();
    }
}
