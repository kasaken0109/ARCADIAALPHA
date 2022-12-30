using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawAttackAction : ActionBase
{
    public string[] StateName = new string[1] { "Attack1" };

    public float[] AnimTime;
    public override bool IsEndAction { get; protected set; }

    public override IEnumerator Execute(Transform target, EnemyAI enemyAI)
    {
        IsEndAction = false;
        var anim = enemyAI.GetComponentInstance<Animator>();
        anim.Play(StateName[0]);
        yield return new WaitForSeconds(1f);
        var combo = Random.Range(0, 3);
        anim.SetInteger("Combo", combo);
        yield return new WaitForSeconds(AnimTime[combo]);
        IsEndAction = true;
    }

    public override void Reset()
    {
        IsEndAction = false;
    }

    public override void Stop()
    {
        IsEndAction = true;
    }
}
