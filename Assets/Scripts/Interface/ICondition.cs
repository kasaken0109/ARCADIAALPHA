using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �������
/// </summary>
public enum ConditionState
{
    Running,
    Success,
    Failure,
}

/// <summary>
/// ����̏���
/// </summary>
public interface ICondition
{
    /// <summary>�����������ǂ���</summary>
    bool IsSuccess { get; set; }

    /// <summary>������e��Ԃ�</summary>
    /// <returns>���茋��</returns>
    ConditionState Check();

    /// <summary>
    /// ����I�����ɍs������������
    /// </summary>
    void Reset();
}
