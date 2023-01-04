using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class MoveAction : ActionBase
{
    public float moveSpeed = 5f;

    NavMeshAgent _navi = default;
    Transform _target = default;
    Animator _anim = default;

    public override bool IsEndAction { get; protected set; }

    public override IEnumerator Execute(Transform target,EnemyAI enemyAI)
    {
        IsEndAction = false;
        _target = enemyAI.GetComponentInstance<Transform>();
        _navi = enemyAI.GetComponentInstance<NavMeshAgent>();
        _navi.destination = target.transform.position;
        _navi.speed = moveSpeed;
        enemyAI.GetComponentInstance<Animator>().SetFloat("Speed", moveSpeed);
        yield return new WaitUntil(() => Vector3.Distance(target.position, _target.position) <= _navi.stoppingDistance + 0.5f);// _navi.isStopped);
        enemyAI.GetComponentInstance<Animator>().SetFloat("Speed", 0);
        IsEndAction = true;
    }

    public override void Reset()
    {
        IsEndAction = false;
    }

    public override void Stop()
    {
        _navi.SetDestination(_target.position);
        _anim.SetFloat("Speed", 0);
        IsEndAction = true;
    }
}
