using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OptionControler : MonoBehaviour
{
    [SerializeField]
    Slider _se;

    [SerializeField]
    Slider _bgm;

    [SerializeField]
    Slider _camera;

    [SerializeField]
    Selectable _selectable;

    EventSystem eventSystem;
    void OnEnable()
    {
        Debug.Log("Awake");
        eventSystem = FindObjectOfType<EventSystem>();
        _se.Select();
        ///音量をsliderの値に反映させる
        _se.value = SoundManager.Instance.GetSEVolume();
        _bgm.value = SoundManager.Instance.GetBGMVolume();
        
    }

    void OnDisable()
    {
        _selectable.Select();
        PlayerPrefs.SetFloat("CameraX", _camera.value);
        PlayerPrefs.SetFloat("CameraY", _camera.value);
        PlayerPrefs.Save();

        FindObjectOfType<OptionData>()?.SetSensitivity(new Vector2(_camera.value, _camera.value));
    }

    /// <summary>
    /// 効果音の音量指定関数を呼び出す
    /// </summary>
    public void SetSE()
    {
        SoundManager.Instance.SetSEVolume(_se.value);
    }

    /// <summary>
    /// BGMの音量指定関数を呼び出す
    /// </summary>
    public void SetBGM()
    {
        SoundManager.Instance.SetBGMVolume(_bgm.value);
        Debug.Log(_bgm.value);
    }

    public void SetCameraSens()
    {
        PlayerPrefs.SetFloat("CameraX", _camera.value);
        PlayerPrefs.SetFloat("CameraY", _camera.value);
        PlayerPrefs.Save();

        FindObjectOfType<OptionData>()?.SetSensitivity(new Vector2(_camera.value,_camera.value));
    }
}
