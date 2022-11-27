using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum EquipSceneState
{
    EquipMain,
    BulletSelect,
    SkillSelect,
    Default,
    SwordSelect
}
public class EquipChangeManager : MonoBehaviour
{
    [SerializeField]
    PanelAnimationController _panelAnimation = default;
    [SerializeField]
    UIAnimationController[] _uIAnimations = default;
    [SerializeField]
    GameObject[] _defaultSelectObject = null;
    [SerializeField]
    GameObject[] _defaultPanel;
    [SerializeField]
    Animator _lonaDisplay;

    public EquipSceneState SceneState => _sceneState;
    EquipSceneState _sceneState = EquipSceneState.Default;
    GameObject prevSelected = null;
    DisplayCameraSwitcher cameraSwitcher;
    List<IUnhinderable> unhinderables = new List<IUnhinderable>();

    // Start is called before the first frame update
    void Awake()
    {
        cameraSwitcher = FindObjectOfType<DisplayCameraSwitcher>();
        cameraSwitcher.SetFocus(FocusState.Lona);
        SetState(3);
    }
    public void SetState(int equipId)
    {
        if (unhinderables.Count() > 0)
        {
            if (unhinderables.All(c => c.IsHinderable())) unhinderables.Clear();
            else {return; }
        }
        var equipSceneState = (EquipSceneState)equipId;
        if(equipId > 2)
        {
            if (_sceneState == equipSceneState) return;
            switch (equipSceneState)
            {
                case EquipSceneState.Default:
                    var prev = _sceneState == EquipSceneState.EquipMain ? 2 : 3;
                    if (_sceneState == EquipSceneState.SwordSelect) _lonaDisplay.SetBool("IsDisplay", false);
                    if (_sceneState == EquipSceneState.EquipMain) cameraSwitcher.SetFocus(FocusState.Lona);
                        EventSystem.current.SetSelectedGameObject(_defaultSelectObject[3]);
                    _uIAnimations[prev].NonActive();
                    unhinderables.Add(_uIAnimations[prev].GetComponent<IUnhinderable>());
                    break;
                case EquipSceneState.SwordSelect:
                    _lonaDisplay.SetBool("IsDisplay", true);
                    _defaultPanel[1].SetActive(true);
                    _uIAnimations[3].Active();
                    unhinderables.Add(_uIAnimations[3].GetComponent<IUnhinderable>());
                    break;
                default:
                    break;
            }
        }
        if (_sceneState != equipSceneState)
        {
            switch (equipSceneState)
            {
                case EquipSceneState.EquipMain:
                    var target = (_sceneState == EquipSceneState.BulletSelect)? _uIAnimations[0] : _uIAnimations[1];
                    _defaultPanel[0].SetActive(true);
                    _panelAnimation.BackPanel();
                    _panelAnimation.SetEnableDisplayChange(false);
                    unhinderables.Add(_panelAnimation.GetComponent<IUnhinderable>());
                    if(_sceneState == EquipSceneState.Default)cameraSwitcher.SetFocus(FocusState.Drone);
                    if(_sceneState != EquipSceneState.Default)
                    {
                        target.NonActive();
                        unhinderables.Add(target.GetComponent<IUnhinderable>());
                    }
                    EventSystem.current.SetSelectedGameObject(prevSelected == null ? _defaultSelectObject[equipId] : prevSelected);

                    break;
                case EquipSceneState.BulletSelect:
                    if (_sceneState == EquipSceneState.EquipMain)
                    {
                        _panelAnimation.FadePanel();
                        unhinderables.Add(_panelAnimation.GetComponent<IUnhinderable>());
                        _uIAnimations[0].gameObject.SetActive(true);
                        _panelAnimation.SetEnableDisplayChange(true);
                        unhinderables.Add(_uIAnimations[0].GetComponent<IUnhinderable>());
                        prevSelected = EventSystem.current.currentSelectedGameObject;
                        EventSystem.current.SetSelectedGameObject(_defaultSelectObject[equipId]);
                    }
                    break;
                case EquipSceneState.SkillSelect:
                    if (_sceneState == EquipSceneState.EquipMain)
                    {
                        _panelAnimation.FadePanel();
                        unhinderables.Add(_panelAnimation.GetComponent<IUnhinderable>());
                        _uIAnimations[1].gameObject.SetActive(true);
                        _panelAnimation.SetEnableDisplayChange(true);
                        unhinderables.Add(_uIAnimations[1].GetComponent<IUnhinderable>());
                        prevSelected = EventSystem.current.currentSelectedGameObject;
                        EventSystem.current.SetSelectedGameObject(_defaultSelectObject[equipId]);
                    }
                    break;
                default:
                    break;
            }
            _sceneState = equipSceneState;
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(_defaultSelectObject[equipId]);
        }
    }
}
