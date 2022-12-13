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
    GameObject _unlockPanel = default;
    private void Start()
    {
        ServiceLocator.GetInstance<EquipmentView>().SetBulletDetailExplainInformations();
    }
    /// <summary>
    /// 選択した弾の情報を装備、表示管理するクラスに送る
    /// </summary>
    public void DataSet(int bulletID)
    {
        var equipPresenter = ServiceLocator.GetInstance<EquipDataPresenter>();
        var bullet = equipPresenter.GetBulletData(bulletID);
        if (!equipPresenter.GetBulletData(bulletID).IsUnlock)
        {
            EquipmentManager.Instance.SetCurrentSelectedBullet(bullet);
            _unlockPanel.SetActive(true);
            return;
        }
        equipPresenter.SendBulletData(bulletID);
        ServiceLocator.GetInstance<EquipmentView>().SetInformations();
    }

    /// <summary>
    /// 選択した弾の情報を装備、表示管理するクラスに送る
    /// </summary>
    public void SkillDataSet(int skillID)
    {
        var equipPresenter = ServiceLocator.GetInstance<EquipDataPresenter>();
        equipPresenter.SendSkillData(skillID);
        ServiceLocator.GetInstance<EquipmentView>().SetInformations();
    }
}
