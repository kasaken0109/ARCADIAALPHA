using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum EquipSceneState
{
    EquipMain,
    BulletSelect,
    SkillSelect
}
public class EquipChangeManager : MonoBehaviour
{
    [SerializeField]
    PanelAnimationController _panelAnimation = default;
    [SerializeField]
    UIAnimationController[] _uIAnimations = default;
    [SerializeField]
    GameObject[] _defaultSelectObject = null;

    EquipSceneState _sceneState = EquipSceneState.EquipMain;
    EventSystem eventSystem;
    GameObject prevSelected = null;
    // Start is called before the first frame update
    void Awake()
    {
        eventSystem = FindObjectOfType<EventSystem>();
        SetState(0);
    }
    public void SetState(int equipId)
    {
        var equipSceneState = (EquipSceneState)equipId;
        if (_sceneState != equipSceneState)
        {
            switch (equipSceneState)
            {
                case EquipSceneState.EquipMain:
                    var target = _sceneState == EquipSceneState.BulletSelect ? _uIAnimations[0] : _uIAnimations[1];
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
