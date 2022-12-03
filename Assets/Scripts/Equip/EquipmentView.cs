using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentView : MonoBehaviour
{
    [SerializeField]
    [Tooltip("�\��UI���X�g")]
    EquipmentInformation[] _equipmentInformations = default;

    [Header("�������f�[�^")]
    [Header("�\�\�\�\�\")]
    [SerializeField]
    Sprite _defaultImage = default;

    [SerializeField]
    BulletList _initList = default;

    [SerializeField]
    string _defaultText = "���ݒ�";

    void Start()
    {
        SetInformations();
    }
    public void SetInformation(int id)
    {
        if (!EquipmentManager.Instance.Equipments[id]) EquipmentManager.Instance.Equipments[id] = _initList.Bullets[id];
        var instance = EquipmentManager.Instance.Equipments[id];
        _equipmentInformations[id].BulletImage.sprite = instance ? instance.Image : _defaultImage;
        _equipmentInformations[id].BulletName.text = instance ? instance.Name : _defaultText;
        _equipmentInformations[id].SkillName.text = instance ? instance.passiveSkill ? instance.PassiveSkill.SkillName : _defaultText : _defaultText;
        _equipmentInformations[id].SkillImage.sprite = instance ? instance.passiveSkill ? instance.PassiveSkill.ImageBullet : _defaultImage : _defaultImage;
    }

    /// <summary>
    /// �����\����ʂ̓��e�����݂̑����󋵂ɍX�V����
    /// </summary>
    public void SetInformations()
    {
        for (int i = 0; i < _equipmentInformations.Length; i++)
        {
            SetInformation(i);
        }
    }
}
