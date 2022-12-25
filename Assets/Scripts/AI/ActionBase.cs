using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionBase
{
    public abstract bool IsEndAction { get; protected set; }

    public abstract IEnumerator Execute(Transform target,EnemyAI enemyAI);

    public abstract void Stop();

    public abstract void Reset();
}
