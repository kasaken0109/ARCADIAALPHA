using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �U���̏�������
/// </summary>
[CreateAssetMenu]
public class AttackData : ScriptableObject
{
    [SerializeField]
    [Tooltip("�L���ɂ���R���C�_�[��ID")]
    int _activeColliderIndex = 0;

    [SerializeField]
    [Tooltip("�L���ɂ��鎞��")]
    float _activeDuraration = 0.5f;

    [SerializeField]
    [Tooltip("�U���̕␳�l")]
    float _attackRate = 1.0f;

    public int ActiveColliderIndex => _activeColliderIndex;
    public float ActiveDuarration => _activeDuraration;
    public float AttackRate => _attackRate;
}
