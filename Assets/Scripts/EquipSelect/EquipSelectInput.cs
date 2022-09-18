using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class EquipSelectInput : MonoBehaviour
{
    [SerializeField]
    [Tooltip("�V�[����߂�{�^��")]
    Button _back;

    [SerializeField]
    [Tooltip("�N�G�X�g�ɏo������{�^��")]
    Button _goQuest;

    /// <summary>UI��playerInput</summary>
    [SerializeField]
    PlayerInput _playerInput;

    PanelAnimationController _animController;

    void Awake()
    {
        TryGetComponent(out _animController);
    }
    private void OnEnable()
    {
        //_playerInput.actions["Back"].started += OnBackScene;
        //_playerInput.actions["GoQuest"].started += OnGoQuest;
        _playerInput.actions["Hold"].started += OnMovePanel;
        _playerInput.actions["Hold"].performed += OnHold;
        _playerInput.actions["Hold"].canceled += OnBackPanel;
    }

    private void OnDisable()
    {
        //_playerInput.actions["Back"].started -= OnBackScene;
        //_playerInput.actions["GoQuest"].started -= OnGoQuest;
        _playerInput.actions["Hold"].started -= OnMovePanel;
        _playerInput.actions["Hold"].performed -= OnHold;
        _playerInput.actions["Hold"].canceled -= OnBackPanel;
    }

    /// <summary>
    /// �V�[����߂�{�^���ɓo�^����Ă��鏈�������s����
    /// </summary>
    /// <param name="obj"></param>
    private void OnBackScene(InputAction.CallbackContext obj)
    {
        _back.onClick?.Invoke();
    }

    /// <summary>
    /// �N�G�X�g�ɍs���{�^���ɓo�^����Ă��鏈�������s����
    /// </summary>
    /// <param name="obj"></param>
    private void OnGoQuest(InputAction.CallbackContext obj)
    {
        _goQuest.onClick?.Invoke();
    }

    private void OnHold(InputAction.CallbackContext obj)
    {
        _animController.ButtonHold();
    }

    private void OnMovePanel(InputAction.CallbackContext obj)
    {
        _animController.DisplayPanelButtonHold();
    }

    private void OnBackPanel(InputAction.CallbackContext obj)
    {
        _animController.HidePanal();
    }
}
