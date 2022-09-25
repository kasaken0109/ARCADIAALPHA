using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentPresenter : MonoBehaviour
{
    [SerializeField]
    EquipmentInformation[] _equipmentInformations = default;

    [Header("初期化データ")]
    [Header("―――――")]
    [SerializeField]
    Sprite _defaultImage = default;

    [SerializeField]
    private string _defaultText = "未設定";

    // Start is called before the first frame update
    void Start()
    {
        //SetInformations();
    }
    public void SetInformation(int id)
    {
        var instance = EquipmentManager.Instance.Equipments[id];
        _equipmentInformations[id].BulletImage.sprite = instance ? instance.Image : _defaultImage;
        _equipmentInformations[id].BulletName.text = instance ? instance.Name : _defaultText;
        _equipmentInformations[id].SkillName.text = instance ? instance.passiveSkill ? instance.PassiveSkill.SkillName : _defaultText : _defaultText;
        _equipmentInformations[id].SkillImage.sprite = instance ? instance.passiveSkill ? instance.PassiveSkill.ImageBullet : _defaultImage : _defaultImage;
    }

    public void SetInformations()
    {
        for (int i = 0; i < _equipmentInformations.Length; i++)
        {
            SetInformation(i);
        }
    }
}
