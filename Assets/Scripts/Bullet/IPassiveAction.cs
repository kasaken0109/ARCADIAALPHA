using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IPassiveAction
{
    bool IsEnd { get; set; }

    void OnAwake();
    void Execute(GameObject set);
    void Execute(int value = 0);
}
