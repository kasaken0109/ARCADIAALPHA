using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class MoveAction : ActionBase
{
    public float moveSpeed = 5f;

    public float moveTime = 5f;

    public float Tolerance = 0.5f;

    NavMeshAgent _navi = default;
    Transform _target = default;
    Animator _anim = default;
    float timer = 0;
    float distance = 0;
    public override bool IsEndAction { get; protected set; }

    public override IEnumerator Execute(Transform target,EnemyAI enemyAI)
    {
        IsEndAction = false;
        _target = enemyAI.GetComponentInstance<Transform>();
        _navi = enemyAI.GetComponentInstance<NavMeshAgent>();
        _navi.isStopped = false;
        _navi.destination = target.transform.position;
        _navi.speed = moveSpeed;
        enemyAI.GetComponentInstance<Animator>().SetFloat("Speed", moveSpeed);
        do
        {
            distance = Vector3.Distance(target.position, _target.position);
            timer += Time.deltaTime;
            yield return null;
        }
        while (distance >= _navi.stoppingDistance + Tolerance || timer < moveTime);
        enemyAI.GetComponentInstance<Animator>().SetFloat("Speed", 0);
        _navi.destination = _target.transform.position;
        _navi.isStopped = true;
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
        _navi.isStopped = true;
        IsEndAction = true;
    }
}
