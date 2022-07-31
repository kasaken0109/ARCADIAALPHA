using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class AddSlipDamage:IPassiveAction
{
    [SerializeField]
    [Tooltip("�X���b�v�_���[�W")]
    int _slipDamage = 5;
    [SerializeField]
    [Tooltip("�X���b�v�_���[�W�̊Ԋu")]
    float _slipInterval = 2f;
    [SerializeField]
    [Tooltip("�X���b�v�_���[�W�������������")]
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
