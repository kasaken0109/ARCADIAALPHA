using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipSetter : MonoBehaviour
{

    public void DataSetter(int value)
    {
        EquipmentManager.Instance.SetEquipID(value);
    }
    public static void DataSetters(int value)
    {
        EquipmentManager.Instance.SetEquipID(value);
    }
}
