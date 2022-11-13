using System.Collections;
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

    public EquipSceneState SceneState => _sceneState;
    EquipSceneState _sceneState = EquipSceneState.EquipMain;
    EventSystem eventSystem;
    GameObject prevSelected = null;

    // Start is called before the first frame update
    void Awake()
    {
        eventSystem = FindObjectOfType<EventSystem>();
        SetState(3);
    }
    public void SetState(int equipId)
    {
        var equipSceneState = (EquipSceneState)equipId;
        if(equipId > 2)
        {
            if (_sceneState == equipSceneState) return;
            switch (equipSceneState)
            {
                case EquipSceneState.Default:
                    var prev = _sceneState == EquipSceneState.EquipMain ? 2 : 3;
                    EventSystem.current.SetSelectedGameObject(_defaultSelectObject[3]);
                    _uIAnimations[prev].NonActive();
                    break;
                case EquipSceneState.SwordSelect:
                    _defaultPanel[1].SetActive(true);
                    _uIAnimations[3].Active();
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
                    var target = (_sceneState == EquipSceneState.BulletSelect || _sceneState == EquipSceneState.Default)? _uIAnimations[0] : _uIAnimations[1];
                    _defaultPanel[0].SetActive(true);
                    _panelAnimation.BackPanel();
                    _panelAnimation.SetEnableDisplayChange(false);
                    target.NonActive();
                    EventSystem.current.SetSelectedGameObject(prevSelected == null ? _defaultSelectObject[equipId] : prevSelected);

                    break;
                case EquipSceneState.BulletSelect:
                    if (_sceneState == EquipSceneState.EquipMain)
                    {
                        _panelAnimation.FadePanel();
                        _uIAnimations[0].gameObject.SetActive(true);
                        _panelAnimation.SetEnableDisplayChange(true);
                        prevSelected = EventSystem.current.currentSelectedGameObject;
                        EventSystem.current.SetSelectedGameObject(_defaultSelectObject[equipId]);
                    }
                    break;
                case EquipSceneState.SkillSelect:
                    if (_sceneState == EquipSceneState.EquipMain)
                    {
                        _panelAnimation.FadePanel();
                        _uIAnimations[1].gameObject.SetActive(true);
                        _panelAnimation.SetEnableDisplayChange(true);
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
        //_defaultSelectObject[equipId].Select();
    }
}
