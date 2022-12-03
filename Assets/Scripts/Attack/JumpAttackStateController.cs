using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class JumpAttackStateController : StateMachineBehaviour
{
    public bool CanMove;
    Action OnEnterListener;
    Action OnUpdateListener;
    Action OnExitListener;

    public void SetStateEnterAction(Action action)
    {
        OnEnterListener += action;
    }

    public void SetStateUpdateAction(Action action)
    {
        OnUpdateListener += action;
    }

    public void SetStateExitAction(Action action)
    {
        OnExitListener += action;
    }
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        OnEnterListener?.Invoke();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        OnUpdateListener?.Invoke();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        OnExitListener?.Invoke();
    }
}
