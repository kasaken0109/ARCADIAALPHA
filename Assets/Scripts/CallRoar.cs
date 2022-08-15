using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Timeline—p‚Ì™ôšK‚ÌU“®‚ğs‚¤
/// </summary>
public class CallRoar : MonoBehaviour
{
    void OnEnable()
    {
        MotorShaker.Instance.Call(ShakeType.Roar);
    }
}
