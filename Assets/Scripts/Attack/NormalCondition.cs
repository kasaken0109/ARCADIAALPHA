using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
/// <summary>
/// 指定時間内に条件がTrueになるか判別する
/// </summary>
public class NormalCondition : ICondition
{
    /// <summary>判定開始時間</summary>
    public float _beginPoint = 0.5f;

    /// <summary>判定秒数</summary>
    public float _checkDuraration = 0.5f;

    /// <summary>判定が成功したかどうか</summary>
    bool isSuccess = false;

    /// <summary>判定が成功したかどうか</summary>
    public bool IsSuccess { get => isSuccess; set => isSuccess = value; }

    /// <summary>経過時間</summary>
    float timer = 0;

    ConditionState state = ConditionState.Success;

    public ConditionState Check()
    {
        timer += Time.deltaTime;
        //Debug.Log($"timer:{timer},_beginPoint:{_beginPoint}");
        //Debug.Log(isSuccess);
        if (timer >= _beginPoint && timer <= _beginPoint + _checkDuraration && isSuccess)
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
