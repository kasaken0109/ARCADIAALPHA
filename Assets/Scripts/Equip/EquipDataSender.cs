using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 選択した弾の情報を装備、表示管理するクラスに送る
/// </summary>
public class EquipDataSender : MonoBehaviour
{
    [SerializeField]
    [Tooltip("情報を送る弾")]
    Bullet _bullet = default;
    [SerializeField]
    [Tooltip("装備の情報を表示する")]
    EquipmentView _equipmentView = default;

    /// <summary>
    /// 選択した弾の情報を装備、表示管理するクラスに送る
    /// </summary>
    public void DataSet()
    {
        EquipmentManager.Instance.SetEquipments(_bullet);
        _equipmentView.SetInformations();
    }
}
