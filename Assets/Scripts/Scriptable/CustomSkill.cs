using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "PassiveSkill")]
public class CustomSkill:ScriptableObject
{
    public enum SkillType
    {
        Passive,
        Buf,
    }

    [SerializeReference,SubclassSelector]
    private IPassiveAction customSkillAction = default;

    [SerializeField]
    [Tooltip("消費コスト")]
    [Range(-1, 1)]
    private float consumeCost = 0f;

    [SerializeField]
    [Tooltip("スキルのタイプ")]
    private SkillType skillType = SkillType.Passive;

    [SerializeField]
    [Tooltip("説明文")]
    private string explainText = default;

    [SerializeField]
    [Tooltip("スキル名")]
    private string skillName = default;

    [SerializeField]
    [Tooltip("スキルイメージ")]
    private Sprite image = default;

    [SerializeField]
    [Tooltip("使用時のエフェクト")]
    GameObject m_effect = default;

    public IPassiveAction CustomSkillAction => customSkillAction;

    public SkillType Type => skillType;

    public float ConsumeCost => consumeCost;

    public string ExplainText => explainText;

    public Sprite ImageBullet => image;

    public string SkillName => skillName;

    public GameObject Effect => m_effect;
}

public enum PassiveType
{
    SwordAttackBuf,
    DefenceBuf,
    MoveSpeedBuf,
    AttackSpeedBuf,
    BulletAttackBuf,
    DodgeDistanceBuf,
    DodgeTimeBuf,
    AttackReachBuf,
}
