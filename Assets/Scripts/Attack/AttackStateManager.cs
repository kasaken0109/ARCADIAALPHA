using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackStateManager : MonoBehaviour
{
    [SerializeField]
    IInputBehavior[] inputBehaviors;

    IInputBehavior currentMotionMap;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartAttack()
    {
        while (!currentMotionMap.IsEnd)
        {
            currentMotionMap.Execute();
        }
    }

    public void SwitchMotionMap(int index = 0)
    {
        currentMotionMap = inputBehaviors[index];
    }
}
