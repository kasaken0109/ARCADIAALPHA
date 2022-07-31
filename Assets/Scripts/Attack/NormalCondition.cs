using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 指定時間内に条件がTrueになるか判別する
/// </summary>
public class NormalCondition : ICondition
{
    /// <summary>判定開始時間</summary>
    public float _beginPoint = 0.5f;

    /// <summary>判定秒数</summary>
    public float _checkDuraration = 1f;

    /// <summary>判定が成功したかどうか</summary>
    bool isSuccess = false;

    /// <summary>判定が成功したかどうか</summary>
    public bool IsSuccess { get => isSuccess; set => isSuccess = value; }

    /// <summary>経過時間</summary>
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
