using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InformationPresenter : MonoBehaviour
{
    public static InformationPresenter Instance { get => _instance; }

    private static InformationPresenter _instance;

    [SerializeField]
    private Text _InformExplain = default;
    [SerializeField]
    private Text _InformName = default;
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
        _InformName.text = "�@�\�� : " + bullet.Name;
        _InformCost.text = "�R�X�g : " + (bullet.ConsumeStanceValue * FixRate).ToString();
        _InformExplain.text = bullet.ExplainText;
    }

    public void SetExplanation(CustomSkill skill)
    {
        if (!skill) return;
        _InformName.text = "�@�\�� : " + skill.SkillName;
        _InformCost.text = "�R�X�g : " + (skill.ConsumeCost * FixRate).ToString();
        _InformExplain.text = skill.ExplainText;
    }
}
