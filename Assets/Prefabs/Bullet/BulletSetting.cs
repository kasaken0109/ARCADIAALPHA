using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BulletSetting")]
public class BulletSetting : ScriptableObject
{
    [SerializeField]
    private float criticalDistance = 30f;

    [SerializeField]
    private bool hasCriticalDistance = false;

    [SerializeField]
    private float reduceDamagePerDistance = 0.1f; 

    public float CriticalDistance => criticalDistance;
    public bool HasCriticalDistance => hasCriticalDistance;

    public float ReduceDamagePerDistance => reduceDamagePerDistance;
}
