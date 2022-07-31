using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

/// <summary>
/// UI�̃R���g���[���[�̓��͂ɉ����ăV�[���J�ڂ̏������s��
/// </summary>
public class UISelectInput : MonoBehaviour
{
    [SerializeField]
    [Tooltip("�V�[����߂�{�^��")]
    Button _back;

    [SerializeField]
    [Tooltip("�N�G�X�g�ɏo������{�^��")]
    Button _goQuest;

    /// <summary>UI��playerInput</summary>
    PlayerInput _playerInput;

    void Awake()
    {
        TryGetComponent(out _playerInput);
    }
    private void OnEnable()
    {
        _playerInput.actions["Back"].started += OnBackScene;
        _playerInput.actions["GoQuest"].started += OnGoQuest;
    }

    private void OnDisable()
    {
        _playerInput.actions["Back"].started -= OnBackScene;
        _playerInput.actions["GoQuest"].started -= OnGoQuest;
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
}
