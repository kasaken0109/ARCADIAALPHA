using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class AddSlipDamage:IPassiveAction
{
    [SerializeField]
    [Tooltip("スリップダメージ")]
    int _slipDamage = 5;
    [SerializeField]
    [Tooltip("スリップダメージの間隔")]
    float _slipInterval = 2f;
    [SerializeField]
    [Tooltip("スリップダメージが発生する期間")]
    float _slipDuraration = 10f;
    public bool IsEnd { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public void Execute(int value = 0)
    {
        EnemyBossManager.Instance.AddSlipDamage(_slipDamage, _slipInterval, _slipDuraration);
    }
    public void OnAwake()
    {
        throw new System.NotImplementedException();
    }

    public void Execute(GameObject set)
    {
        set.GetComponentInParent<EnemyBossManager>().AddSlipDamage(_slipDamage, _slipInterval, _slipDuraration);
    }
}
