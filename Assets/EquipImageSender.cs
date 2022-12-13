using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EquipDisplayState
{
    NonEquip,
    Equip1,
    Equip2,
    Equip3,
}
public class EquipImageSender : MonoBehaviour
{
    [SerializeField]
    Sprite[] _equipStateSptite = default;

    [SerializeField]
    Image _stateDisplay = default;
    EquipDisplayState _state = EquipDisplayState.NonEquip;

    public void SetEquipState(EquipDisplayState equipDisplayState)
    {
        _state = equipDisplayState;
        _stateDisplay.sprite = _equipStateSptite[(int)_state];
    }

    public EquipDisplayState GetEquipState()
    {
        return _state;
    }
}
