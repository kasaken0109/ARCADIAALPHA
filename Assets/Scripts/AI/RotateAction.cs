using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAction : ActionBase
{
    public float RotateSpeed = 0.1f;

    public float AngleTolerance = 0.1f;

    float angle = 0f;
    public override bool IsEndAction { get; protected set; }

    public override IEnumerator Execute(Transform target, EnemyAI enemyAI)
    {
        var enemyT = enemyAI.GetComponentInstance<Transform>();
        var anim = enemyAI.GetComponentInstance<Animator>();
        Vector3 relative = enemyT.InverseTransformPoint(target.transform.position);
        angle = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg;
        //Debug.Log(Mathf.Abs(angle));
        while(Mathf.Abs(angle) > AngleTolerance)
        {
            relative = enemyT.InverseTransformPoint(target.transform.position);
            angle = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg;
            enemyT.Rotate(0, angle > 0 ? RotateSpeed * anim.speed : -RotateSpeed * anim.speed, 0);
            anim.Play("Rotate");
            yield return null;
        }
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
