using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarriaController : MonoBehaviour,ICustomSkillEvent
{
    [SerializeField]
    string _opponentTag = "Enemy";

    bool isHitFromEnemy = false;

    GameObject hitObj;

    public Action<GameObject> CustomSkillEvent { get => _customSkillEvent; set => _customSkillEvent = value; }
    Action<GameObject> _customSkillEvent;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(_opponentTag))
        {
            hitObj = other.gameObject;
            isHitFromEnemy = true;
        }
    }
    public void ActiveCustomSkill()
    {
        if (!isHitFromEnemy) return;       
        _customSkillEvent?.Invoke(hitObj);
        isHitFromEnemy = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(_opponentTag) && other.gameObject == hitObj)
        {
            isHitFromEnemy = false;
        }
    }
}
