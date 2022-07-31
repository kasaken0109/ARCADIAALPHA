using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>���ʉ��AAudioMixer���t�B�[���h�Ɏ��N���X</summary>
[CreateAssetMenu(menuName = "SoundAssets")]
public class SoundAssets : ScriptableObject
{
    /// <summary>BGM�p��AudioMixer</summary>
    public AudioMixerGroup _audioMixerBGM = default;
    /// <summary>SE�p��AudioMixer</summary>
    public AudioMixerGroup _audioMixerSE = default;

    /// <summary>�N���b�N��</summary>
    public AudioClip[] m_click;
    public AudioClip m_frostWall;
    public AudioClip m_playerHit;
    public AudioClip m_roar;
    public AudioClip m_shoot;
    public AudioClip m_heal;
    public AudioClip m_fireB;
    public AudioClip m_move;
    public AudioClip m_dodge;
    public AudioClip m_kick;
    public AudioClip m_blizzard;
    public AudioClip m_god;
    public AudioClip m_charge;
    public AudioClip m_emptyBullet;
    [Range(-80, 20)] public float m_seVolume = -60f;
    [Range(-80, 20)] public float m_bgmVolume = -60f;
}
