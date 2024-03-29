using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPosRetention : MonoBehaviour
{
    [SerializeField]
    [Tooltip("当たった場所")]
    Transform _hitpos;
    public Transform HitPos => _hitpos != null ? _hitpos : transform;
}
