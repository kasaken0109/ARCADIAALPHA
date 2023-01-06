using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.InputSystem;

public class TutorialPlayerInput : MonoBehaviour
{
    PlayerInput _playerInput;
    PlayerMoveController _playerMove;
    PlayerAttackController _playerAttack;
    BulletFire _bulletFire;
    BulletSelectController _bulletSelectController;
    CameraController _cameraController;
    OptionData _optionData;
    //TutorialType _preTutorialType = TutorialType.Look;

    Vector3 dir;

    private void Awake()
    {
        TryGetComponent(out _playerInput);
        TryGetComponent(out _playerMove);
        TryGetComponent(out _playerAttack);
        _optionData = FindObjectOfType<OptionData>();
        _bulletFire = FindObjectOfType<BulletFire>();
        _bulletSelectController = FindObjectOfType<BulletSelectController>();
        _cameraController = FindObjectOfType<CameraController>();
    }

    private void Start()
    {
        ServiceLocator.GetInstance<TutorialManager>().TutorialActionStateChanged.Subscribe(x => SetTutorialMove(x));
    }


    void OnEnable()
    {
        _optionData.CamaraResume();
    }

    private void OnDisable()
    {
        _optionData.CamaraStop();
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
        //if (!_tutorialType.HasFlag((TutorialType)2)) return;
        //Move();
    }

    private void Move()
    {
        var direction = _playerInput.actions["Move"].ReadValue<Vector2>();
        dir = new Vector3(direction.x, 0, direction.y);
        _playerMove.Move(dir);
    }

    public void SetTutorialMove(TutorialType tutorialType)
    {
        switch (tutorialType)
        {
            case TutorialType.None:
                this.enabled = false;
                break;
            case TutorialType.Look:
                _playerInput.actions["LockOn"].performed += OnLockOn;
                break;
            case TutorialType.Move:
                _playerInput.actions["Option"].started += OnMenu;
                Observable.EveryFixedUpdate().Subscribe(_ => Move());
                break;
            case TutorialType.Attack:
                _playerInput.actions["Attack"].started += OnAttack;
                break;
            case TutorialType.Jump:
                _playerInput.actions["Jump"].started += OnJump;
                break;
            case TutorialType.Dodge:
                _playerInput.actions["Dodge"].started += OnDodge;
                break;
            case TutorialType.Fire:
                _playerInput.actions["Fire"].started += OnFire;
                _playerInput.actions["Fire"].canceled += OnFireCanceled;
                _playerInput.actions["Equip1"].started += OnEquip1;
                _playerInput.actions["Equip2"].started += OnEquip2;
                _playerInput.actions["Equip3"].started += OnEquip3;
                break;
        };
    }
    private void OnJump(InputAction.CallbackContext obj)
    {
        _playerMove.Jump();
    }

    bool IsFireing = false;
    private void OnFire(InputAction.CallbackContext obj)
    {
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
        _playerMove.Dodge(dir);
    }
    private void OnAttack(InputAction.CallbackContext obj)
    {
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
