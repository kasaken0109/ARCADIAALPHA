using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �I�������e�̏��𑕔��A�\���Ǘ�����N���X�ɑ���
/// </summary>
public class EquipDataSender : MonoBehaviour
{

    private void Start()
    {
        ServiceLocator.GetInstance<EquipmentView>().SetBulletDetailExplainInformations();
    }
    /// <summary>
    /// �I�������e�̏��𑕔��A�\���Ǘ�����N���X�ɑ���
    /// </summary>
    public void DataSet(int bulletID)
    {
        var equipPresenter = ServiceLocator.GetInstance<EquipDataPresenter>();
        equipPresenter.SendBulletData(bulletID);
        ServiceLocator.GetInstance<EquipmentView>().SetInformations();
    }

    /// <summary>
    /// �I�������e�̏��𑕔��A�\���Ǘ�����N���X�ɑ���
    /// </summary>
    public void SkillDataSet(int skillID)
    {
        var equipPresenter = ServiceLocator.GetInstance<EquipDataPresenter>();
        equipPresenter.SendSkillData(skillID);
        ServiceLocator.GetInstance<EquipmentView>().SetInformations();
    }
}
