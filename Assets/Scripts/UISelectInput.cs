using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

/// <summary>
/// UIのコントローラーの入力に応じてシーン遷移の処理を行う
/// </summary>
public class UISelectInput : MonoBehaviour
{
    [SerializeField]
    [Tooltip("シーンを戻るボタン")]
    Button _back;

    [SerializeField]
    [Tooltip("クエストに出発するボタン")]
    Button _goQuest;

    /// <summary>UIのplayerInput</summary>
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
}
