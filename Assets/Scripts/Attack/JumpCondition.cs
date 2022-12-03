using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class JumpCondition : ICondition
{
    /// <summary>”»’èŠJnŠÔ</summary>
    public float _beginPoint = 0.5f;

    /// <summary>”»’è•b”</summary>
    public float _checkDuraration = 0.5f;

    /// <summary>”»’è‚ª¬Œ÷‚µ‚½‚©‚Ç‚¤‚©</summary>
    bool isSuccess = false;

    /// <summary>”»’è‚ª¬Œ÷‚µ‚½‚©‚Ç‚¤‚©</summary>
    public bool IsSuccess { get => isSuccess; set => isSuccess = value; }

    /// <summary>Œo‰ßŠÔ</summary>
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
