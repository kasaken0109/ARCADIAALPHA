using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InformationPresenter : MonoBehaviour
{
    public static InformationPresenter Instance { get => _instance; }

    private static InformationPresenter _instance;

    [SerializeField]
    private Text _SkillName = default;
    [SerializeField]
    private Text _BulletName = default;
    [SerializeField]
    private Text _InformCost = default;

    static readonly float FixRate = 100f;

    private void Awake()
    {
        _instance = this;
    }
    public void SetExplanation(Bullet bullet) 
    {
        if (!bullet) return;
        _BulletName.text = "�@�\�� : " + bullet.Name;
        _InformCost.text = "�R�X�g : " + (bullet.ConsumeStanceValue * FixRate).ToString();
    }

    public void SetExplanation(CustomSkill skill)
    {
        if (!skill) return;
        _BulletName.text = "�@�\�� : " + skill.SkillName;
        _SkillName.text = skill.ExplainText;
    }
}
