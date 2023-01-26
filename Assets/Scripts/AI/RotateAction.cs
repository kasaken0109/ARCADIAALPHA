using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;

public class RotateAction : ActionBase
{
    public float AngleTolerance = 0.1f;

    public float RotateTime = 10f;

    float angle = 0f;
    NavMeshAgent _navi;
    public override bool IsEndAction { get; protected set; }

    public override IEnumerator Execute(Transform target, EnemyAI enemyAI)
    {
        var enemyT = enemyAI.GetComponentInstance<Transform>();
        var anim = enemyAI.GetComponentInstance<Animator>();
        _navi = enemyAI.GetComponentInstance<NavMeshAgent>();
        _navi.isStopped = true;
        Vector3 relative = enemyT.InverseTransformPoint(target.transform.position);
        angle = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg;
        anim.SetBool("IsRotate",true);
        enemyT.DORotate(new Vector3(0, angle, 0), RotateTime,RotateMode.LocalAxisAdd)
        .OnComplete(() =>
        {
            anim.SetBool("IsRotate",false);
            angle = 0;
            IsEndAction = true;
        });
        yield return null;
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
