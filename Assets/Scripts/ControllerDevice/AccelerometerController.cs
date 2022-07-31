using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// コントローラーの加速度に応じた処理を行う
/// </summary>
public class AccelerometerController : MonoBehaviour
{
    private void OnEnable()
    {
        //加速度センサーを有効化
        InputSystem.EnableDevice(Accelerometer.current);
        StartCoroutine(AccelerationValueUpdate());
    }

    private void Awake()
    {
        //加速度センサーを有効化
        InputSystem.EnableDevice(Accelerometer.current);
        StartCoroutine(AccelerationValueUpdate());
    }

    private void OnDisable()
    {
        //加速度センサーを無効化
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
