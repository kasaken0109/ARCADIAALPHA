using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.InputSystem;
using System.Linq;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public enum PlayerState
{
    Default,
    OnField,
    InAir,
    Invisible,
    Stun,
}
/// <summary>
/// プレイヤーのパラメーター(体力、ムテキ時間等)を管理するクラス
/// </summary>
[RequireComponent(typeof(PlayerMoveController))]
public class PlayerManager : MonoBehaviour,IDamage
{
    public static PlayerManager Instance { get; private set; }

    [SerializeField]
    [Tooltip("プレイヤーの体力")]
    int _hp = 500;

    [SerializeField]
    [Tooltip("回避時の無敵時間")]
    float _godTime = 1f;

    [SerializeField]
    [Tooltip("回避成功時の無敵時間")]
    float _changeTime = 10f;

    [SerializeField]
    [Tooltip("回避成功時の無敵時間")]
    float _invisibleTimeOnDodge = 2f;

    [SerializeField]
    [Tooltip("回避成功時のスロウ率")]
    float _slowRate = 0.1f;

    [SerializeField]
    [Tooltip("吹っ飛び時の無敵時間")]
    float _invisibleOnBlowTime = 6f;

    [SerializeField]
    [Tooltip("回復時に発生するエフェクト")]
    GameObject _healEffect;

    [SerializeField]
    [Tooltip("死亡時に発生するプレイヤーの死体")]
    GameObject _deadBody;

    [SerializeField]
    [Tooltip("体力赤バー")]
    Image _hpslider = null;

    [SerializeField]
    [Tooltip("体力緑バー")]
    Image _hpsliderGreen = null;

    [SerializeField]
    [Tooltip("画面の演出用Volume")]
    Volume _volume;
    
    /// <summary>現在のプレイヤーのHP</summary>
    int _maxhp;
    /// <summary>ムテキ状態か</summary>
    bool IsInvisible = false;
    /// <summary>プレイヤーのアニメーター</summary>
    Animator _anim = null;
    /// <summary>playerControll</summary>
    PlayerMoveController _playerControll;
    /// <summary>ムテキ時間を管理するバフ値</summary>
    BufferParameter _invisibleTime;

    PlayerState playerState = PlayerState.OnField;

    public PlayerState PlayerState {
        get => playerState;
        set
        {
            //優先度が高いものが入っていた場合はその値の保持を優先する
            if(playerState < value) playerState = value;
            else
            {
                if (playerState < PlayerState.Invisible) playerState = value;
            }
        }
    }
    
    VolumeProfile _volumeProfile;
    ChromaticAberration chromaticAberration;
    ColorAdjustments colorAjustments;

    const int bigDamageAmount = 50;

    void Awake()
    {
        Instance = this;
        _invisibleTime = new BufferParameter(_godTime);
        _maxhp = _hp;
        TryGetComponent(out _playerControll);
        TryGetComponent(out _anim);
        InitDisplayEffect();
    }

    /// <summary>
    /// 画面のエフェクトの初期化を行う
    /// </summary>
    void InitDisplayEffect()
    {
        _volumeProfile = _volume.profile;
        chromaticAberration = _volume.profile.Add<ChromaticAberration>(true);
        chromaticAberration.intensity.value = 1f;//追加して値を変更
        colorAjustments = _volume.profile.Add<ColorAdjustments>(true);
        _volume.weight = 0f;
    }

    public void AddDamage(int damage,ref GameObject call)
    {
        if (damage < 0)
        {
            Heal(damage);
        }
        else if (IsInvisible)
        {
            StartCoroutine(DodgeSuccess());
        }
        else
        {
            Hit(damage);
        }
        _hpsliderGreen.DOFillAmount((float)_hp / _maxhp,
                0f).OnComplete(() =>
                {
                    _hpslider.DOFillAmount((float)_hp / _maxhp,
                        1f);
                });
    }

    /// <summary>
    /// プレイヤーの体力に応じて行う処理を変更する
    /// </summary>
    /// <param name="damage">ダメージ値</param>
    void Hit(int damage)
    {
        if (_hp > damage)
        {
            _hp -= damage;
            OnHit(damage);
        }
        else
        {
            _hp = 0;
            OnDead();
        }
    }

    /// <summary>
    /// 攻撃ヒット時の処理を行う
    /// </summary>
    /// <param name="damage">ダメージ値</param>
    void OnHit(int damage)
    {
        _anim.Play(damage > bigDamageAmount ? "BigDamage" : "Damage");
        MotorShaker.Instance.Call(ShakeType.Damage);
        SoundManager.Instance.PlayPlayerHit();
    }

    /// <summary>
    /// 死んだときの処理を行う
    /// </summary>
    void OnDead()
    {
        Instantiate(_deadBody, transform.position, transform.rotation);
        GameManager.Instance.SetGameState(GameState.PLAYERLOSE);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// プレイヤーを回復する
    /// </summary>
    /// <param name="damage">回復量(マイナス値)</param>
    void Heal(int damage)
    {
        _hp -= damage;
        if (_hp >= _maxhp) _hp = _maxhp;
        Instantiate(_healEffect, transform.position, Quaternion.identity);
        SoundManager.Instance.PlayHeal();
    }

    /// <summary>
    /// 回避時のムテキ時間の処理を開始する(AnimationEvent用)
    /// </summary>
    public void SetInvisible()
    {
        StartCoroutine(nameof(Invisible));
    }

    /// <summary>
    /// 吹っ飛び時のムテキ時間の処理を開始する(AnimationEvent用)
    /// </summary>
    public void SetInvisibleOnBlow()
    {
        StartCoroutine(Invisible(_invisibleOnBlowTime));
    }

    /// <summary>
    /// ムテキ時間を変更する
    /// </summary>
    /// <param name="invisibleRate">変更割合</param>
    /// <param name="time">変更時間</param>
    public void ChangeInvisibleTime(float invisibleRate, float time)
    {
        StartCoroutine(_invisibleTime.ChangeValue(invisibleRate, time));
    }

    /// <summary>回避可能かどうか</summary>
    bool ActiveDodge = false;
    /// <summary>
    /// 通常回避時に呼ばれるプレイヤームテキ処理
    /// </summary>
    /// <returns></returns>
    IEnumerator Invisible()
    {
        IsInvisible = true;
        ActiveDodge = true;
        yield return new WaitForSeconds(_invisibleTime.Value);
        IsInvisible = false;
        ActiveDodge = false;
    }

    const int normalLayer = 8;
    const int invisibleLayer = 6;
    /// <summary>
    /// 吹っ飛び時に呼ばれるプレイヤームテキ処理
    /// </summary>
    /// <param name="time">ムテキ時間</param>
    /// <returns></returns>
    IEnumerator Invisible(float time)
    {
        IsInvisible = true;
        gameObject.layer = normalLayer;
        yield return new WaitForSeconds(time);
        gameObject.layer = invisibleLayer;
        IsInvisible = false;
    }

    /// <summary>
    /// 回避成功時の画面と敵に与える処理を行う
    /// </summary>
    /// <returns></returns>
    IEnumerator DodgeSuccess()
    {
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        enemies = enemies.Where(x => x.GetComponent<EnemyBossManager>()).ToArray();
        List<Animator> animators = new List<Animator>();

        foreach (var item in enemies)
        {
            animators.Add(item.GetComponent<Animator>());
        }
        animators.ForEach(x => {
            x.speed = _slowRate;
        });
        //DOTween.To(() => _volume.weight, (x) => _volume.weight = x, 1, 0.5f);
        playerState = PlayerState.Invisible;
        yield return new WaitForSeconds(_changeTime);
        //DOTween.To(() => _volume.weight, (x) => _volume.weight = x, 0, 0.5f);
        if(animators.Count != 0)animators.ForEach(x => x.speed = 1);
        playerState = PlayerState.Default;
    }
}
