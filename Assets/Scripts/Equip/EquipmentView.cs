using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentView : MonoBehaviour
{
    [SerializeField]
    [Tooltip("�\��UI���X�g")]
    EquipmentInformation[] _equipmentInformations = default;

    [SerializeField]
    EquipmentDetailInformation[] _equipmentDetailInformations = default;

    [Header("�������f�[�^")]
    [Header("�\�\�\�\�\")]
    [SerializeField]
    Sprite _defaultImage = default;

    [SerializeField]
    string _defaultText = "���ݒ�";

    private void Awake()
    {
        ServiceLocator.SetInstance<EquipmentView>(this);
    }
    void Start()
    {
        SetInformations();
    }
    public void SetInformation(int id)
    {
        var instance = EquipmentManager.Instance.Equipments[id];
        //
        _equipmentInformations[id].BulletImage.sprite = instance ? instance.Image : _defaultImage;
        _equipmentInformations[id].BulletName.text = instance ? instance.Name : _defaultText;
        _equipmentInformations[id].SkillName.text = instance ? instance.passiveSkill ? instance.PassiveSkill.SkillName : _defaultText : _defaultText;
        _equipmentInformations[id].SkillImage.sprite = instance ? instance.passiveSkill ? instance.PassiveSkill.ImageBullet : _defaultImage : _defaultImage;
    }

    public void SetBulletDetailExplainInformations()
    {
        var instance = ServiceLocator.GetInstance<EquipDataPresenter>();
        for (int i = 0; i < instance.GetBulletLength(); i++)
        {
            _equipmentDetailInformations[i].BulletName.text = instance.GetBulletData(i).Name;
            _equipmentDetailInformations[i].BulletExplainContext.text = instance.GetBulletData(i).ExplainText;
        }
    }

    public void SetSkillDetailExplainInformations()
    {
        var instance = ServiceLocator.GetInstance<EquipDataPresenter>();
        for (int i = 0; i < instance.GetBulletLength(); i++)
        {
            _equipmentDetailInformations[i].SkillName.text = instance.GetSkillData(i).SkillName;
            _equipmentDetailInformations[i].SkillExplainContext.text = instance.GetSkillData(i).ExplainText;
        }
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

    private void OnDestroy()
    {
        ServiceLocator.RemoveInstance<EquipmentView>();
    }
}
