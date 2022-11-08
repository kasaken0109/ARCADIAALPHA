using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IPassiveAction
{
    bool IsEnd { get; set; }

    void OnAwake();
    void Execute(GameObject set);
    void Execute(float value = 0);

    float GetEffectiveValue();

    float GetEffectiveTime();
}
