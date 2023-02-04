using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Linq;

public enum FocusState
{
    Lona,
    Drone
}
public class DisplayCameraSwitcher : MonoBehaviour
{
    [SerializeField]
    CinemachineVirtualCameraBase[] _focusCameras = default;

    const int DefaultPriority = 0;
    const int HighPriority = 100;

    int[] originPrioritys;

    private void Awake()
    {
        originPrioritys = new int[_focusCameras.Length];
        for (int i = 0; i < originPrioritys.Length; i++)
        {
            originPrioritys[i] = _focusCameras[i].Priority;
        }
    }
    public void SetFocus(int focusState)
    {
        for (int i = 0; i < _focusCameras.Length; i++)
        {
            _focusCameras[i].Priority = i == (int)focusState ? HighPriority : originPrioritys[i];
        }
    }

    public void ResetFocus(int focusState)
    {
        for (int i = 0; i < _focusCameras.Length; i++)
        {
            _focusCameras[focusState].Priority = originPrioritys[i];
        }
    }
}
