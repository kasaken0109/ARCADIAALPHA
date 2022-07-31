using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �w�莞�ԓ��ɏ�����True�ɂȂ邩���ʂ���
/// </summary>
public class NormalCondition : ICondition
{
    /// <summary>����J�n����</summary>
    public float _beginPoint = 0.5f;

    /// <summary>����b��</summary>
    public float _checkDuraration = 1f;

    /// <summary>���肪�����������ǂ���</summary>
    bool isSuccess = false;

    /// <summary>���肪�����������ǂ���</summary>
    public bool IsSuccess { get => isSuccess; set => isSuccess = value; }

    /// <summary>�o�ߎ���</summary>
    float timer = 0;

    ConditionState state;

    public ConditionState Check()
    {
        timer += Time.deltaTime;
        
        if (timer >= _beginPoint && timer <= _beginPoint + _checkDuraration && isSuccess)
        {
            state = ConditionState.Success;
        }
        else
        {
            if(timer > _beginPoint + _checkDuraration)
            {
                state = ConditionState.Failure;
            }
            else
            {
                state = ConditionState.Running;
            }
        }
        //Debug.Log($"State:{state}, Time :{timer}");
        return state;
    }

    public void Reset()
    {
        isSuccess = false;
        timer = 0;
    }
}
