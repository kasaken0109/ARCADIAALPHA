using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Timeline�p�̙��K���̐U�����s��
/// </summary>
public class CallRoar : MonoBehaviour
{
    void OnEnable()
    {
        MotorShaker.Instance.Call(ShakeType.Roar);
    }
}
