using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UniRx;
public enum TutorialType
{
    None,
    Look,
    Move,
    Attack,
    Jump,
    Dodge,
    Fire,
    All,
}

[System.Serializable]
public class TutorialTypeReactiveProperty : ReactiveProperty<TutorialType>
{
    public TutorialTypeReactiveProperty(){}
    public TutorialTypeReactiveProperty(TutorialType init):base(init) { }
}
/// <summary>
/// �`���[�g���A���̊Ǘ����s��
/// </summary>
public class TutorialManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("�`���[�g���A���J�n���ɑI����Ԃɂ���{�^��")]
    Button _focusButtonOnTutorialStart = default;
    [SerializeField]
    [Tooltip("�`���[�g���A���J�n��ɑI����Ԃɂ���I�u�W�F�N�g")]
    GameObject _prevFocus = default;
    [SerializeField]
    [Tooltip("�`���[�g���A���X�L�b�v���Ɏ��s�����C�x���g")]
    UnityEvent _OnTutorialSkip = default;

    [SerializeField]
    TutorialPlayerInput _tutorialPlayerInput = default;
    /// <summary>�I����ԂɂȂ��Ă���{�^��</summary>
    GameObject _prev = default;
    /// <summary>�`���[�g���A�����X�L�b�v���ꂽ��</summary>
    bool isTutorialSkipped = false;

    TutorialTypeReactiveProperty _tutorialActionType = new TutorialTypeReactiveProperty();
    TutorialTypeReactiveProperty _tutorialNavigationType = new TutorialTypeReactiveProperty();
    IReactiveProperty<TutorialType> _tutorialReactiveProperty;

    public IObservable<TutorialType> TutorialActionStateChanged => _tutorialActionType;
    public IObservable<TutorialType> TutorialNavigationStateChanged => _tutorialNavigationType;

    private void Awake()
    {
        //�T�[�r�X���P�[�^�[�փC���X�^���X��o�^
        ServiceLocator.SetInstance(this);
        ButtonSelect();
        _tutorialActionType.Value = TutorialType.Look;
        _tutorialActionType.Value = TutorialType.Move;
        //Observable.Interval(TimeSpan.FromSeconds(0.2)).Subscribe(x => { _tutorialType.Value = (TutorialType)x + 1;});
    }

    private void OnDestroy()
    {
        //�T�[�r�X���P�[�^�[�̃C���X�^���X���폜
        ServiceLocator.RemoveInstance<TutorialManager>();
    }

    public void ChangeTutorialType(TutorialType tutorialType)
    {
        _tutorialActionType.Value = tutorialType;
    }

    /// <summary>
    /// �`���[�g���A���V�[���J�n���Ƀ{�^����I����Ԃɂ���
    /// </summary>
    void ButtonSelect()
    {
        //�C�x���g�V�X�e���ɑI������Ă����I�u�W�F�N�g���L��
        _prev = EventSystem.current.currentSelectedGameObject ?? _prevFocus; 
        EventSystem.current.SetSelectedGameObject(_focusButtonOnTutorialStart.gameObject);
    }

    public void ResetFocusSelectable()
    {
        //�I����Ԃ̃I�u�W�F�N�g�������ꍇ�{�^��UI�L�����ɑI�΂��I�u�W�F�N�g��ݒ�
        if (!_prev) _prev = EventSystem.current.currentSelectedGameObject;
        //�I����Ԃ����Z�b�g
        EventSystem.current.SetSelectedGameObject(_prev);
    }

    /// <summary>
    /// �`���[�g���A���X�L�b�v���Ɏ��s����
    /// </summary>
    public void OnSkipTutorial()
    {
        //�C�x���g�����s
        _OnTutorialSkip?.Invoke();
        isTutorialSkipped = true;
    }
}
