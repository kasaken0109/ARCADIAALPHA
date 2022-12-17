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
    [Tooltip("�����̏ڍ׏���\��������UI���X�g")]
    EquipmentDetailInformation[] _equipmentDetailInformations = default;

    [SerializeField]
    [Tooltip("�����̃X���b�g������\��������")]
    EquipImageSender[] _equiplist = default;

    [Header("�������f�[�^")]
    [Header("�\�\�\�\�\")]
    [SerializeField]
    [Tooltip("�f�t�H���g�摜")]
    Sprite _defaultImage = default;

    [SerializeField]
    [Tooltip("�f�t�H���g�e�L�X�g")]
    string _defaultText = "���ݒ�";

    private void Awake()
    {
        //�C���X�^���X��o�^
        ServiceLocator.SetInstance<EquipmentView>(this);
    }
    void Start()
    {
        SetInformations();
        SetBulletDetailExplainInformations();
        SetSkillDetailExplainInformations();
    }

    public void SetEquipIcons()
    {
        var equip = EquipmentManager.Instance.Equipments;
        for (int i = 0; i < _equiplist.Length; i++)
        {
             _equiplist[i].SetEquipState((EquipDisplayState)0);
            for (int k = 0; k < equip.Length; k++)
            {
                if (equip[k].BulletID == i) _equiplist[i].SetEquipState((EquipDisplayState)k + 1);
            }
        }
        
    }

    public void SetEquipIcon(int id,int equipDisplayState)
    {
        _equiplist[id].SetEquipState((EquipDisplayState)equipDisplayState);
    }
    public void SetInformation(int id)
    {
        var instance = EquipmentManager.Instance.Equipments[id];
        //
        _equipmentInformations[id].BulletImage.sprite = instance.Image ?? _defaultImage;
        _equipmentInformations[id].BulletName.text = instance.Name ?? _defaultText;
        _equipmentInformations[id].SkillName.text = instance.PassiveSkill.SkillName ?? _defaultText;
        _equipmentInformations[id].SkillImage.sprite = instance.PassiveSkill.ImageBullet ?? _defaultImage;
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
        for (int i = 0; i < instance.GetSkillLength(); i++)
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
