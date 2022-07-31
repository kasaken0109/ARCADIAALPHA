using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface ICustomSkillEvent
{
    public Action<GameObject> CustomSkillEvent { get; set; }
}
