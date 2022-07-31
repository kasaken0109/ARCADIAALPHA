using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuffAction
{
    public bool IsEnd { get; set; }
    public void Execute(GameObject targetCollider);
}
