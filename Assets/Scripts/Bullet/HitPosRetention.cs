using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPosRetention : MonoBehaviour
{
    [SerializeField]
    [Tooltip("“–‚½‚Á‚½êŠ")]
    Transform _hitpos;
    public Transform HitPos => _hitpos != null ? _hitpos : transform;
}
