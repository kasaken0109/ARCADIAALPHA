using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class JumpCondition : ICondition
{
    /// <summary>����J�n����</summary>
    public float _beginPoint = 0.5f;

    /// <summary>����b��</summary>
    public float _checkDuraration = 0.5f;

    /// <summary>���肪�����������ǂ���</summary>
    bool isSuccess = false;

    /// <summary>���肪�����������ǂ���</summary>
    public bool IsSuccess { get => isSuccess; set => isSuccess = value; }

    /// <summary>�o�ߎ���</summary>
    float timer = 0;

    ConditionState state = ConditionState.Success;

    public ConditionState Check()
    {
        timer += Time.deltaTime;
        //Debug.Log($"timer:{timer},_beginPoint:{_beginPoint}");
        //Debug.Log(isSuccess);
        if (timer >= _beginPoint && timer <= _beginPoint + _checkDuraration && isSuccess && PlayerManager.Instance.PlayerState == PlayerState.InAir)
        {
            //Debug.Log($"timer:{timer},_beginPoint:{_beginPoint}");
            state = ConditionState.Success;
        }
        else
        {
            //Debug.Log($"timer:{timer},_beginPoint:{_beginPoint}");
            if (timer > _beginPoint + _checkDuraration)
            {
                state = ConditionState.Failure;
            }
            else
            {
                state = ConditionState.Running;
            }
        }
        //Debug.Log($"State:{state}, Time :{timer}{isSuccess}");
        return state;
    }

    public void Reset()
    {
        //Debug.Log("Reset");
        isSuccess = false;
        timer = 0;
    }
}
