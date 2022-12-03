using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBarria : MonoBehaviour, ICustomSkillEvent
{
    public Action<GameObject> CustomSkillEvent { get => _customSkillEvent; set => _customSkillEvent = value; }
    Action<GameObject> _customSkillEvent;

    // Start is called before the first frame update
    void Start()
    {
        PlayerManager.Instance.GenerateBarria(_customSkillEvent);   
    }
}
