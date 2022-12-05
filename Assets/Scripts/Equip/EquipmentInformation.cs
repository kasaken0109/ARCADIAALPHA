using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class EquipmentInformation
{
    public Image BulletImage;

    public Text BulletName;

    public Image SkillImage;

    public Text SkillName;
}


[System.Serializable]
public class EquipmentDetailInformation
{
    public Text BulletName;

    public Text BulletExplainContext;

    public Text SkillName;

    public Text SkillExplainContext;

}
