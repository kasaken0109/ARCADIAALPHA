using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Bullets/Create Bullet")]
public class Bullet : ScriptableObject
{
    [SerializeField]
    [Tooltip("生成する弾")]
    GameObject m_bullet;

    [SerializeField]
    [Tooltip("弾のダメージ")]
    int m_damage = 1;

    [SerializeField]
    [Tooltip("弾の攻撃持続時間")]
    float _attackDuraration = 0;

    [SerializeField]
    [Tooltip("クールダウンタイム")]
    float m_delay = 0.2f;

    [SerializeField]
    [Tooltip("消費するスタンス値")]
    [Range(-1, 1)]
    private float m_consumeStanceValue;

    [Tooltip("カスタムスキル")]
    public CustomSkill passiveSkill = default;

    [SerializeField]
    [Tooltip("弾のタイプ")]
    private BulletType m_bulletType = default;

    [SerializeReference, SubclassSelector]
    [Tooltip("実行する処理")]
    private IPassiveAction _passiveAction = default;

    [SerializeField]
    private string m_name = default;

    [SerializeField]
    private string m_explainText = default;

    [SerializeField]
    private Sprite m_image = default;


    int m_bulletID = 0;
    public GameObject MyBullet => m_bullet;

    public int Damage => m_damage;

    public float AttackDuraration => _attackDuraration;

    public IPassiveAction PassiveAction => _passiveAction;

    public float ConsumeStanceValue => m_consumeStanceValue;

    public CustomSkill PassiveSkill { get =>passiveSkill;  set { passiveSkill = value; } }


    public BulletType BulletType => m_bulletType;

    public int BulletID => m_bulletID;

    public float Delay => m_delay;

    public string Name => m_name;

    public Sprite Image => m_image;

    public string ExplainText => m_explainText;
}
