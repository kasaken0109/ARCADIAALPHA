using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class OptionData : MonoBehaviour
{
    [SerializeField]
    CinemachineVirtualCamera virtualCamera;
    float cameraSensitivityX = 1f;

    float cameraSensitivityY = 1f;

    CinemachinePOV POV;

    private void Awake()
    {
        POV = virtualCamera.GetCinemachineComponent<CinemachinePOV>();
        var hol = PlayerPrefs.GetFloat("CameraX");
        var ver = PlayerPrefs.GetFloat("CameraY");
        SetSensitivity(new Vector2(hol, ver));
    }
    public void SetSensitivity(Vector2 sensitivity)
    {
        POV.m_HorizontalAxis.m_MaxSpeed = sensitivity.x;
        POV.m_VerticalAxis.m_MaxSpeed = sensitivity.y;
    }

    public void CamaraStop()
    {
        POV.m_HorizontalAxis.m_MaxSpeed = 0;
        POV.m_VerticalAxis.m_MaxSpeed = 0;
    }

    public void CamaraResume()
    {
        POV.m_HorizontalAxis.m_MaxSpeed = PlayerPrefs.GetFloat("CameraX");
        POV.m_VerticalAxis.m_MaxSpeed = PlayerPrefs.GetFloat("CameraY");
    }


}

