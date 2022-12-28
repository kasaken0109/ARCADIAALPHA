using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public enum TutorialType
{
    None = 0,
    Look = 1,
    Move = 2,
    Attack = 4,
    Jump = 8,
    Dodge = 16,
    Fire = 32,
    All = 63,
}
public class TutorialPlayerInput : MonoBehaviour
{
    PlayerInput _playerInput;
    PlayerMoveController _playerMove;
    PlayerAttackController _playerAttack;
    BulletFire _bulletFire;
    BulletSelectController _bulletSelectController;
    CameraController _cameraController;
    OptionData _optionData;
    TutorialType _tutorialType = TutorialType.Look;
    TutorialType _preTutorialType = TutorialType.Look;

    Vector3 dir;

    private void Awake()
    {
        _tutorialType = _tutorialType | TutorialType.Move;
        TryGetComponent(out _playerInput);
        TryGetComponent(out _playerMove);
        TryGetComponent(out _playerAttack);
        _optionData = FindObjectOfType<OptionData>();
        _bulletFire = FindObjectOfType<BulletFire>();
        _bulletSelectController = FindObjectOfType<BulletSelectController>();
        _cameraController = FindObjectOfType<CameraController>();
    }


    void OnEnable()
    {
        _playerInput.actions["Jump"].started += OnJump;
        _playerInput.actions["Fire"].started += OnFire;
        _playerInput.actions["Fire"].canceled += OnFireCanceled;
        _playerInput.actions["Dodge"].started += OnDodge;
        _playerInput.actions["Attack"].started += OnAttack;
        _playerInput.actions["Option"].started += OnMenu;
        _playerInput.actions["Equip1"].started += OnEquip1;
        _playerInput.actions["Equip2"].started += OnEquip2;
        _playerInput.actions["Equip3"].started += OnEquip3;
        _playerInput.actions["LockOn"].performed += OnLockOn;
        // NewTest a = new NewTest();
        //a.Player.Move.performed += contexet => _playerControll.Move(Vector3.zero); 
    }

    private void OnDisable()
    {
        _playerInput.actions["Jump"].started -= OnJump;
        _playerInput.actions["Fire"].started -= OnFire;
        _playerInput.actions["Fire"].canceled += OnFireCanceled;
        _playerInput.actions["Dodge"].started -= OnDodge;
        _playerInput.actions["Attack"].started -= OnAttack;
        _playerInput.actions["Option"].started -= OnMenu;
        _playerInput.actions["Equip1"].started -= OnEquip1;
        _playerInput.actions["Equip2"].started -= OnEquip2;
        _playerInput.actions["Equip3"].started -= OnEquip3;
        _playerInput.actions["LockOn"].performed -= OnLockOn;
    }


    private void FixedUpdate()
    {
        if (!_tutorialType.HasFlag((TutorialType)2)) return;
        var direction = _playerInput.actions["Move"].ReadValue<Vector2>();
        dir = new Vector3(direction.x, 0, direction.y);
        _playerMove.Move(dir);
    }
    public void SetTutorialMove()
    {

    }
    private void OnJump(InputAction.CallbackContext obj)
    {
        if (!_tutorialType.HasFlag((TutorialType)8)) return;
        _playerMove.Jump();
    }

    bool IsFireing = false;
    private void OnFire(InputAction.CallbackContext obj)
    {
        if (!_tutorialType.HasFlag((TutorialType)32)) return;
        IsFireing = true;
        StartCoroutine(AssaltMode());
        IEnumerator AssaltMode()
        {
            while (IsFireing)
            {
                if (_bulletFire.CanFire) _bulletFire.ShootBullet();
                yield return new WaitForSeconds(0.2f);
            }
        }

    }

    public bool IsPressShotKey()
    {
        return IsFireing;
    }

    private void OnFireCanceled(InputAction.CallbackContext obj)
    {
        IsFireing = false;
    }

    private void OnDodge(InputAction.CallbackContext obj)
    {
        if (!_tutorialType.HasFlag((TutorialType)16)) return;
        _playerMove.Dodge(dir);
    }
    private void OnAttack(InputAction.CallbackContext obj)
    {
        if (!_tutorialType.HasFlag((TutorialType)4)) return;
        _playerAttack.AttackSignal();
    }

    private void OnMenu(InputAction.CallbackContext obj)
    {
        GameManager.Instance.SetMenu();
    }

    private void OnEquip1(InputAction.CallbackContext obj)
    {
        _bulletSelectController.SelectBullet(0);
    }

    private void OnEquip2(InputAction.CallbackContext obj)
    {
        _bulletSelectController.SelectBullet(1);
    }

    private void OnEquip3(InputAction.CallbackContext obj)
    {
        _bulletSelectController.SelectBullet(2);
    }

    private void OnLockOn(InputAction.CallbackContext obj)
    {
        Debug.Log("EnterLockON");
        _cameraController.LockON();
    }
}
