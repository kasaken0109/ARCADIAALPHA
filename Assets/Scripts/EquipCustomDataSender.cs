using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipCustomDataSender : MonoBehaviour
{
    [SerializeField]
    CustomSkill _skill;
    [SerializeField]
    EquipmentView _equipmentView = default;
    // Start is called before the first frame update
    public void DataSet()
    {
        EquipmentManager.Instance.Equipments[EquipmentManager.Instance.GetEquipID].PassiveSkill = _skill;
        _equipmentView.SetInformations();
    }
}
