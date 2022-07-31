using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInputBehavior
{
    public bool IsEnd { get; }
    public void Execute();
}
