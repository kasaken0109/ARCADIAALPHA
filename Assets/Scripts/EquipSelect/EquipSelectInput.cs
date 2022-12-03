using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

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
    EventSystem eventSystem;
    EquipSwordController _swordController;
    EquipChangeManager _equipChangeManager;
    [SerializeField]
    SizeRescalerController _sizeRescalerController;

    void Awake()
    {
        TryGetComponent(out _animController);
        eventSystem = FindObjectOfType<EventSystem>();
        _swordController = FindObjectOfType<EquipSwordController>();
        _equipChangeManager = FindObjectOfType<EquipChangeManager>();
        //_sizeRescalerController = FindObjectOfType<SizeRescalerController>();
    }
    private void OnEnable()
    {
        _playerInput.actions["Back"].started += OnBackScene;
        _playerInput.actions["GoQuest"].started += OnGoQuest;
        _playerInput.actions["Hold"].started += OnMovePanel;
        _playerInput.actions["Hold"].performed += OnHold;
        _playerInput.actions["Hold"].canceled += OnBackPanel;
        //_playerInput.actions["Submit"].started += OnSubmit;
        _playerInput.actions["Equip"].started += OnEquip;
    }

    private void OnDisable()
    {
        _playerInput.actions["Back"].started -= OnBackScene;
        _playerInput.actions["GoQuest"].started -= OnGoQuest;
        _playerInput.actions["Hold"].started -= OnMovePanel;
        _playerInput.actions["Hold"].performed -= OnHold;
        _playerInput.actions["Hold"].canceled -= OnBackPanel;
        //_playerInput.actions["Submit"].started -= OnSubmit;
        _playerInput.actions["Equip"].started -= OnEquip;
    }

    private void Start()
    {
    }

    /// <summary>
    /// シーンを戻るボタンに登録されている処理を実行する
    /// </summary>
    /// <param name="obj"></param>
    private void OnBackScene(InputAction.CallbackContext obj)
    {
        if (_equipChangeManager)
        {
            switch (_equipChangeManager.SceneState)
            {
                case EquipSceneState.EquipMain:
                    _equipChangeManager.SetState(3);
                    break;
                case EquipSceneState.BulletSelect:
                    _equipChangeManager.SetState(0);
                    break;
                case EquipSceneState.SkillSelect:
                    _equipChangeManager.SetState(0);
                    break;
                case EquipSceneState.Default:
                    _back.onClick?.Invoke();
                    break;
                case EquipSceneState.SwordSelect:
                    _equipChangeManager.SetState(3);
                    break;
                default:
                    break;
            }
        }
        else
        {
            _back.onClick?.Invoke();
        }
        
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

    private void OnSubmit(InputAction.CallbackContext obj)
    {
       var button =  eventSystem.currentSelectedGameObject.GetComponent<Button>();
    }

    private void OnEquip(InputAction.CallbackContext obj)
    {
        if (_equipChangeManager.SceneState != EquipSceneState.SwordSelect) return;
        var m = obj.ReadValue<float>();
        _swordController.SetWeapon(m > 0);
        _sizeRescalerController.SelectSwordDisplay(m > 0);
    }
}
