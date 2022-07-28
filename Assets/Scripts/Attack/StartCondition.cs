using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCondition : ICondition
{
    bool isSuccess = false;
    public bool IsSuccess { get => isSuccess; set => isSuccess = value; }

    public ConditionState Check()
    {
        return isSuccess ? ConditionState.Success : ConditionState.Running;
    }

    public void Reset()
    {
        isSuccess = false;
    }
}
