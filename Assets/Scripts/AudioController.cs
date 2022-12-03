using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの音の処理を管理する
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class AudioController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("プレイヤーに関する音")]
    AudioClip[] m_sounds;

    /// <summary>使用するAnimator</summary>
    Animator _anim = default;

    /// <summary>使用するAudioSource</summary>
    AudioSource _source;
    
    void Start()
    {
        _source = GetComponents<AudioSource>()[0];
        TryGetComponent(out _source);
        TryGetComponent(out _anim);
    }

    /// <summary>
    /// 足音を鳴らす
    /// </summary>
    public void PlayFootstepSE()
    {
        if (_anim.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
        {
            _source.PlayOneShot(m_sounds[0]);
        }
        else if (_anim.GetCurrentAnimatorStateInfo(0).IsName("Run"))
        {
            _source.PlayOneShot(m_sounds[1]);
        }
    }

    /// <summary>
    /// ジャンプ音を鳴らす
    /// </summary>
    public void PlayJumpSE()
    {
        _source.PlayOneShot(m_sounds[2]);
    }

    /// <summary>
    /// 近接通常攻撃の音を鳴らす
    /// </summary>
    public void PlaySlashSE()
    {
        _source.PlayOneShot(m_sounds[3]);
    }

    /// <summary>
    /// 近接特殊攻撃の音を鳴らす
    /// </summary>
    public void PlaySpecialSlashSE()
    {
        _source.PlayOneShot(m_sounds[4]);
    }
}
