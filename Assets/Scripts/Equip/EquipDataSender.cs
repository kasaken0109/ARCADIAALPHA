using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �I�������e�̏��𑕔��A�\���Ǘ�����N���X�ɑ���
/// </summary>
public class EquipDataSender : MonoBehaviour
{
    [SerializeField]
    [Tooltip("���𑗂�e")]
    Bullet _bullet = default;
    [SerializeField]
    [Tooltip("�����̏���\������")]
    EquipmentView _equipmentView = default;

    /// <summary>
    /// �I�������e�̏��𑕔��A�\���Ǘ�����N���X�ɑ���
    /// </summary>
    public void DataSet()
    {
        EquipmentManager.Instance.SetEquipments(_bullet);
        _equipmentView.SetInformations();
    }
}
