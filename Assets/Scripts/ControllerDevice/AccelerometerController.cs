using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// �R���g���[���[�̉����x�ɉ������������s��
/// </summary>
public class AccelerometerController : MonoBehaviour
{
    private void OnEnable()
    {
        //�����x�Z���T�[��L����
        InputSystem.EnableDevice(Accelerometer.current);
        StartCoroutine(AccelerationValueUpdate());
    }

    private void Awake()
    {
        //�����x�Z���T�[��L����
        InputSystem.EnableDevice(Accelerometer.current);
        StartCoroutine(AccelerationValueUpdate());
    }

    private void OnDisable()
    {
        //�����x�Z���T�[�𖳌���
        InputSystem.DisableDevice(Accelerometer.current);
        StopCoroutine(AccelerationValueUpdate());
    }

    IEnumerator AccelerationValueUpdate()
    {
        Debug.Log("EnableUpdate");
        while (Accelerometer.current != null)
        {
            Debug.Log("updateValue");
            Debug.Log(Accelerometer.current.acceleration.ReadValue());
            yield return null;
        }
    }
}
