﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Bullets/Create Bullet")]
public class Bullet : ScriptableObject
{
    [SerializeField]
    bool _isUnlock = true;
    [SerializeField]
    int _requirePointToUnlock = 1;

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
    bool _isPermanence = false;

    [SerializeField]
    [Tooltip("クールダウンタイム")]
    float m_delay = 0.2f;

    [SerializeField]
    [Tooltip("消費するスタンス値")]
    [Range(-1, 1)]
    private float m_consumeStanceValue;

    [SerializeField]
    BulletCustomType _bulletCustomType = BulletCustomType.All;

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

    [SerializeField]
    private Sprite _equipImage = default;

    [SerializeField]
    private Sprite _equipCoolDownImage = default;

    [SerializeField]
    int m_bulletID = 0;

    public bool IsUnlock { get => _isUnlock; set => _isUnlock = value; }

    public int RequirePointToUnlock => _requirePointToUnlock;
    public GameObject MyBullet => m_bullet;

    public int Damage => m_damage;

    public float AttackDuraration => _attackDuraration;

    public bool IsPermanence => _isPermanence;

    public IPassiveAction PassiveAction => _passiveAction;

    public float ConsumeStanceValue => m_consumeStanceValue;

    public BulletCustomType BulletCustomType => _bulletCustomType;

    public CustomSkill PassiveSkill { get =>passiveSkill;  set { passiveSkill = value; } }


    public BulletType BulletType => m_bulletType;

    public int BulletID => m_bulletID;

    public float Delay => m_delay;

    public string Name => m_name;

    public Sprite Image => m_image;

    public Sprite EquipImage => _equipImage;

    public Sprite EquipCoolDownImage => _equipCoolDownImage;


    public string ExplainText => m_explainText;
}
