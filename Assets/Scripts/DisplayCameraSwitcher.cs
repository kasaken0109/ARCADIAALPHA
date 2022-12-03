using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public enum FocusState
{
    Lona,
    Drone
}
public class DisplayCameraSwitcher : MonoBehaviour
{
    [SerializeField]
    CinemachineVirtualCameraBase[] _focusCameras = default;

    const int DefaultPriority = 10;
    const int HighPriority = 20;
    public void SetFocus(FocusState focusState)
    {
        for (int i = 0; i < _focusCameras.Length; i++)
        {
            _focusCameras[i].Priority = i == (int)focusState ? HighPriority : DefaultPriority;
        }
    }
}
