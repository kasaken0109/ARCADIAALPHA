using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class EquipSelectInput : MonoBehaviour
{
    [SerializeField]
    [Tooltip("シーンを戻るボタン")]
    Button _back;

    [SerializeField]
    [Tooltip("クエストに出発するボタン")]
    Button _goQuest;

    /// <summary>UIのplayerInput</summary>
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
    /// シーンを戻るボタンに登録されている処理を実行する
    /// </summary>
    /// <param name="obj"></param>
    private void OnBackScene(InputAction.CallbackContext obj)
    {
        _back.onClick?.Invoke();
    }

    /// <summary>
    /// クエストに行くボタンに登録されている処理を実行する
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
