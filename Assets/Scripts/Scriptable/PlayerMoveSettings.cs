using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Player/Settings")]
public class PlayerMoveSettings : ScriptableObject
{
    [SerializeField]
    [Tooltip("動く速さ")]
    private float m_movingSpeed = 5f;

    [SerializeField]
    [Tooltip("走る速さの補正値")]
    private float m_runningCorrection = 1.6f;

    [SerializeField]
    [Tooltip("ターンの速さ")]
    private float m_turnSpeed = 3f;

    [SerializeField]
    [Tooltip("ジャンプ力")]
    private float m_jumpPower = 5f;

    [SerializeField]
    [Tooltip("滑空時のY軸の落下速度の緩和")]
    [Range(0.01f,0.9f)]
    private float m_floatPower = 0.2f;

    [SerializeField]
    [Tooltip("滞空時の水平移動速度の軽減率")]
    private float m_midairSpeedRate = 0.7f;

    [SerializeField]
    [Tooltip("滞空時のMP減少率")]
    private float m_midairConsumeRate = 0.003f;

    [SerializeField]
    [Tooltip("突進力")]
    private float m_dushPower = 10f;

    [SerializeField]
    [Tooltip("突進攻撃力")]
    private int m_dushAttackPower = 15;

    [SerializeField]
    [Tooltip("回避距離")]
    private float m_dodgeLength = 5;

    [SerializeField]
    [Tooltip("回避のクールダウン")]
    private float m_dodgeCoolDown = 0.3f;

    [SerializeField]
    [Tooltip("animator")]
    private RuntimeAnimatorController m_anim = default;

    public float MovingSpeed => m_movingSpeed;

    public float RunningCorrection => m_runningCorrection;

    public float TurnSpeed => m_turnSpeed;

    public float JumpPower => m_jumpPower;

    public float FloatPower => m_floatPower;

    public float MidairSpeedRate => m_midairSpeedRate;

    public float MidairConsumeRate => m_midairConsumeRate;

    public float DushPower => m_dushPower;

    public int DushAttackPower => m_dushAttackPower;

    public float DodgeLength => m_dodgeLength;

    public float DodgeCoolDown => m_dodgeCoolDown;

    public RuntimeAnimatorController Anim => m_anim;
}
