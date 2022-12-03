using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;

/// <summary>
/// ���j���[���쎞�̋����𐧌䂷��
/// </summary>
public class MenuInput : MonoBehaviour
{
    [SerializeField]
    [Tooltip("PlayerInputAsset")]
    PlayerInput _playerInput = default;

    EventSystem eventSystem;
    // Start is called before the first frame update
    private void OnEnable()
    {
        eventSystem = FindObjectOfType<EventSystem>();
        //_playerInput.SwitchCurrentActionMap("UI");
        _playerInput.actions["Undo"].started += OnCancel;
        _playerInput.actions["SetSlider"].started += OnSetSliderValue;

    }

    private void OnDisable()
    {
        _playerInput.actions["Undo"].started -= OnCancel;
        _playerInput.actions["SetSlider"].started -= OnSetSliderValue;
        //_playerInput.SwitchCurrentActionMap("Player");
    }

    /// <summary>
    /// �J�����p�l�������
    /// </summary>
    /// <param name="obj"></param>
    public void OnCancel(InputAction.CallbackContext obj)
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// slider�̒l���󂯎�����l�ɒ�������
    /// </summary>
    /// <param name="obj"></param>
    public void OnSetSliderValue(InputAction.CallbackContext obj)
    {
       var slider = eventSystem.currentSelectedGameObject.GetComponent<Slider>();
        if(slider)slider.value += obj.ReadValue<float>() * 2;
    }
}
