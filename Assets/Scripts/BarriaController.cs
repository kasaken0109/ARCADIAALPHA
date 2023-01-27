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
    Transform hitPoint;

    public Action<GameObject,Transform> CustomSkillEvent { get => _customSkillEvent; set => _customSkillEvent = value; }
    Action<GameObject,Transform> _customSkillEvent;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(_opponentTag))
        {
            hitObj = other.gameObject;
            hitPoint = other.gameObject.transform;
            isHitFromEnemy = true;
        }
    }
    public void ActiveCustomSkill()
    {
        if (!isHitFromEnemy) return;       
        _customSkillEvent?.Invoke(hitObj,hitPoint);
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
