using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Linq;

/// <summary>
/// 音の処理を管理するクラス
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    /// <summary>
    /// instance、無かったら生成して返す
    /// </summary>
    public static SoundManager Instance {
        get
        {
            var target = FindObjectOfType<SoundManager>();
            if (target)
            {
                instance = target;
            }
            else
            {
                var gm = GameObject.Find("GM");
                if (!gm) gm = new GameObject("GM");
                instance = gm.AddComponent<SoundManager>();
                DontDestroyOnLoad(instance);
            }
            return instance;
        }
    }

    static SoundManager instance = null;

    /// <summary>SEを鳴らすAudioSource</summary>
    AudioSource seSource;

    /// <summary>BGMを鳴らすAudioSource</summary>
    AudioSource bgmSource;

    /// <summary>効果音、AudioMixerをフィールドに持つクラス</summary>
    SoundAssets _soundAssets = null;

    bool IsInint = true;

    void Awake()
    {
        if (IsInint)
        {
            //soundAssetを設定する
            GameObject ins = (GameObject)Resources.Load("SoundAssets");
            _soundAssets = ins.GetComponent<SoundAssetInformation>().SoundAssets;
            //BGMのAudioSourceの初期設定を行う
            var bgm = new GameObject("BGM");
            bgm.transform.SetParent(gameObject.transform);
            bgmSource = bgm.AddComponent<AudioSource>();
            bgmSource.volume = 0.1f;
            bgmSource.loop = true;
            bgmSource.outputAudioMixerGroup= _soundAssets._audioMixerBGM;
            //SEのAudioSourceの初期設定を行う
            seSource = GetComponent<AudioSource>();
            seSource.outputAudioMixerGroup = _soundAssets._audioMixerSE;

            DontDestroyOnLoad(gameObject);
            //Volumeを初期化
            SetSEVolume(_soundAssets.m_seVolume);
            SetBGMVolume(_soundAssets.m_bgmVolume);
            IsInint = false;
        }
    }

    /// <summary>
    /// クリック音を鳴らす
    /// </summary>
    /// <param name="index">鳴らすクリック音ID</param>
    public void PlayClick(int index = 0)
    {
        if (index > _soundAssets.m_click.Length) index = 0;
        seSource.PlayOneShot(_soundAssets.m_click[index]);
    }

    /// <summary>
    /// クリック音を鳴らす
    /// </summary>
    /// <param name="clip">鳴らす音</param>
    public void PlayClick(AudioClip clip)
    {
        seSource.PlayOneShot(clip);
    }

    /// <summary>
    /// 咆哮音を鳴らす
    /// </summary>
    public void PlayRoar()
    {
        seSource.PlayOneShot(_soundAssets.m_roar);
    }
    /// <summary>
    /// キック音を鳴らす
    /// </summary>
    public void PlayKick()
    {
        seSource.PlayOneShot(_soundAssets.m_kick);
    }
    /// <summary>
    /// 移動音を鳴らす
    /// </summary>
    public void PlayMove()
    {
        seSource.PlayOneShot(_soundAssets.m_move);
    }

    /// <summary>
    /// 回復音を鳴らす
    /// </summary>
    public void PlayHeal()
    {
        seSource.PlayOneShot(_soundAssets.m_heal);
    }

    /// <summary>
    /// 氷結音を鳴らす
    /// </summary>
    public void PlayFrost()
    {
        seSource.PlayOneShot(_soundAssets.m_frostWall);
    }
    /// <summary>
    /// 回避音を鳴らす
    /// </summary>
    public void PlayDodge()
    {
        seSource.PlayOneShot(_soundAssets.m_dodge);
    }

    /// <summary>
    /// 回避成功音を鳴らす
    /// </summary>
    public void PlayInvisible()
    {
        seSource.PlayOneShot(_soundAssets.m_god);
    }
    /// <summary>
    /// 射撃音を鳴らす
    /// </summary>
    public void PlayShoot()
    {
        seSource.PlayOneShot(_soundAssets.m_shoot);
    }

    /// <summary>
    /// チャージ音を鳴らす
    /// </summary>
    public void PlayCharge()
    {
        seSource.PlayOneShot(_soundAssets.m_charge);
    }

    /// <summary>
    /// エネルギー切れ音を鳴らす
    /// </summary>
    public void PlayEmptyBullet()
    {
        seSource.PlayOneShot(_soundAssets.m_emptyBullet);
    }

    /// <summary>
    /// SEの再生を止める
    /// </summary>
    public void StopSE()
    {
        seSource.Pause();
    }

    /// <summary>
    /// ヒット時の音を鳴らす
    /// </summary>
    /// <param name="hit">ヒット時鳴らす音</param>
    public void PlayHit(AudioClip hit)
    {
        seSource.PlayOneShot(hit);
    }
    /// <summary>
    /// プレイヤー被弾時の音を鳴らす
    /// </summary>
    public void PlayPlayerHit()
    {
        seSource.PlayOneShot(_soundAssets.m_playerHit);
    }
    /// <summary>
    /// 特定の場所でヒット音を鳴らす
    /// </summary>
    /// <param name="hit">鳴らす音</param>
    /// <param name="pos">場所</param>
    public void PlayHit(AudioClip hit, Vector3 pos)
    {
        AudioSource.PlayClipAtPoint(hit, pos, ConvertDbToVolume()/10);
    }

    /// <summary>
    /// BGMを流す
    /// </summary>
    /// <param name="bgm">流す音楽</param>
    public void PlayBGM(AudioClip bgm)
    {
        if (bgmSource.clip) bgmSource.Stop();
        bgmSource.clip = bgm;
        bgmSource.Play();
    }

    /// <summary>
    /// BGMを止める
    /// </summary>
    public void StopBGM()
    {
        bgmSource.Pause();
    }

    /// <summary>
    /// BGMの再生を再開する
    /// </summary>
    public void ResumeBGM()
    {
        bgmSource.UnPause();
    }

    /// <summary>
    /// ㏈を0-1の値に変換する
    /// </summary>
    /// <returns>変換後の値</returns>
    public float ConvertDbToVolume()
    {
        return Mathf.Abs(GetSEVolume()-80) / 100;
    }

    /// <summary>
    /// SEの音量を調整する
    /// </summary>
    /// <param name="volume">音量</param>
    public void SetSEVolume(float volume)
    {
        _soundAssets._audioMixerSE.audioMixer.SetFloat("MasterVolume", volume);
    }

    /// <summary>
    /// BGMの音量を調整する
    /// </summary>
    /// <param name="volume">音量</param>
    public void SetBGMVolume(float volume)
    {
        _soundAssets._audioMixerBGM.audioMixer.SetFloat("MasterVolume", volume);
    }

    /// <summary>
    /// SEの音量を取得する
    /// </summary>
    /// <returns>SE音量</returns>
    public float GetSEVolume()
    {
        float volume;
        _soundAssets._audioMixerSE.audioMixer.GetFloat("MasterVolume", out volume);
        return volume;
    }

    /// <summary>
    /// BGMの音量を取得する
    /// </summary>
    /// <returns>BGM音量</returns>
    public float GetBGMVolume()
    {
        float volume;
        _soundAssets._audioMixerBGM.audioMixer.GetFloat("MasterVolume", out volume);
        return volume;
    }
}
