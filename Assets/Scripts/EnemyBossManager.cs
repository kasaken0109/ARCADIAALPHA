using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UniRx;
using BehaviourAI;
using System;

/// <summary>
/// 敵の状態を管理する
/// </summary>
public class EnemyBossManager : MonoBehaviour, IDamage,IStun,IFlameBurn,ILockOnTargetable
{
    public static EnemyBossManager Instance { get; private set; }

    [SerializeField]
    [Tooltip("HP")]
    int m_hp = 100;

    [SerializeField]
    GameObject _lockOnIcon = default;

    [SerializeField]
    Transform _camPoint = default;

    [SerializeField]
    [Tooltip("怯み値")]
    [Range(1, 9999)]
    int m_rate;

    [SerializeField]
    [Tooltip("スタン耐久値")]
    int m_stun = 5;

    [SerializeField]
    [Tooltip("スタン時間")]
    float m_stunTime = 8f;

    [SerializeField]
    int _basicAttackPower = 80;

    [SerializeField]
    [Tooltip("毒状態に発生するエフェクト")]
    GameObject _poison = default;

    [SerializeField]
    [Tooltip("Animator")]
    Animator m_animator = null;

    [SerializeField]
    AttackSetController _attackSetController = default;

    [SerializeField]
    [Tooltip("死亡時に発生する死体")]
    GameObject m_deathBody = null;

    [SerializeField]
    [Tooltip("地面から砂が発生する攻撃のエフェクト")]
    GameObject m_sandEffect = null;

    [SerializeField]
    [Tooltip("地面から砂が発生する攻撃のエフェクト")]
    GameObject m_stunEffect = null;

    [SerializeField]
    Image hpSlider;

    [SerializeField]
    RectTransform hpFrame;

    bool IsCritical = false;//特殊攻撃のフラグ

    int maxHp;
    int hitRate = 0;//怯み値
    int rateTemp;
    int count = 0;//特殊攻撃の回数
    int stun;
    float hitstopRate = 0.5f;
    const float coefficient = 8f;
    const float actionHpRateHalf = 0.5f;
    const float actionHpRateLittle = 0.2f;

    Vector3 originHpPos = default;

    GameObject me;

    Cinemachine.CinemachineImpulseSource impulseSource;

    #region EnemyAnimatorHash
    int hpHash = Animator.StringToHash("HP");
    int mpHash = Animator.StringToHash("MP");

    #endregion

    public Subject<Unit> OnResetCam;
    void Awake()
    {
        Instance = this;
        maxHp = m_hp;
        TryGetComponent(out impulseSource);
        me = gameObject;
        stun = m_stun;
        OnResetCam = new Subject<Unit>().AddTo(this);
        originHpPos = hpFrame.transform.position;
    }

    public void AddDamage(int damage,ref GameObject call)
    {
        if (call.CompareTag("Player"))
        {
            var weaponAttribute = call.GetComponent<WeaponAttributeController>();
            StunChecker(weaponAttribute.StunPower);
            FindObjectOfType<BulletFire>().AddStanceValue(weaponAttribute.MPAbsorbValue);
        }
        StopCoroutine(HitStop());
        if (damage >= Mathf.CeilToInt(m_rate * hitstopRate))
        {
            StartCoroutine(HitStop());
            MotorShaker.Instance.Call(ShakeType.Hit, coefficient * damage / maxHp);
        }
        if (m_hp > damage)
        {
            m_hp -= damage;
            UpdateHpDisplay();
            hitRate += damage;
            if (hitRate >= m_rate)
            {
                m_animator.SetInteger(hpHash, 1);
                m_animator.SetTrigger("Hit");
                hitRate = 0;
            }
            return;
        }
        Dead();
    }

    private void UpdateHpDisplay()
    {
        DOTween.To(
                    () => hpSlider.fillAmount, // getter
                    x => hpSlider.fillAmount = x, // setter
                    (float)(float)m_hp / maxHp, // ターゲットとなる値
                    0.3f  // 時間（秒）
                    ).SetEase(Ease.InFlash);
        hpFrame.transform.DOShakePosition(0.5f, 20, 100)
            .OnComplete(() => hpFrame.transform.position = originHpPos);
    }

    private void Dead()
    {
        m_hp = 0;
        StopCoroutine(HitStop());
        Time.timeScale = 1;
        DOTween.To(
            () => hpSlider.fillAmount, // getter
            x => hpSlider.fillAmount = x, // setter
            (float)(float)0, // ターゲットとなる値
            0.3f  // 時間（秒）
            ).SetEase(Ease.InFlash);
        hpSlider.transform.DOShakePosition(0.5f);
        Instantiate(m_deathBody, this.transform.position, this.transform.rotation);
        GameManager.Instance.SetGameState(GameState.PLAYERWIN);
        OnResetCam?.OnNext(Unit.Default);
        Destroy(this.gameObject);
    }

    /// <summary>
    /// ヒットストップを発生させる
    /// </summary>
    /// <returns></returns>
    IEnumerator HitStop()
    {
        impulseSource.GenerateImpulse();
        Time.timeScale = 0.5f;
        yield return new WaitForSeconds(0.1f);
        Time.timeScale = 1;
    }
    IEnumerator SlipDamage(int slipValue, float slipInterval,float slipDuraration,Transform hitPos)
    {
        float time = 0;
        float intervalTime = 0;
        _poison?.SetActive(true);
        while (time <= slipDuraration)
        {
            time += Time.deltaTime;
            intervalTime += Time.deltaTime;
            yield return null;
            if (intervalTime >= slipInterval)
            {
                AddDamage(slipValue,ref me);
                intervalTime = 0;
            }
        }
        _poison?.SetActive(false);

    }
    public void SetAttackColliderEnemy(AttackData attackData)
    {
        _attackSetController.ActiveAttackCollider(attackData.ActiveColliderIndex, attackData.ActiveDuarration, (int)(attackData.AttackRate * _basicAttackPower));
    }
    public void StunChecker(int value)
    {
        stun = stun >= value ? stun -= value : 0;
        if (stun == 0 && !m_stunEffect.activeInHierarchy)
        {
            Stun();
            stun = m_stun;
        }
    }

    void Stun()
    {
        IEnumerator StunCoroutine()
        {
            m_animator.SetBool("IsStun",true);
            m_stunEffect.SetActive(true);
            GetComponent<EnemyAI>().SetAIActive(false);
            yield return new WaitForSeconds(m_stunTime);
            GetComponent<EnemyAI>().SetAIActive(true);
            m_stunEffect.SetActive(false);
            m_animator.SetBool("IsStun", false);
        }
        StartCoroutine(StunCoroutine());
    }

    public void AddSlipDamage(int slipValue, float slipInterval, float slipDuraration,Transform hitPos)
    {
        StartCoroutine(SlipDamage(slipValue,slipInterval,slipDuraration,hitPos));
    }
    public void ShowLockOnIcon()
    {
        _lockOnIcon.SetActive(true);
    }

    public Transform GetCamPoint()
    {
        return _camPoint.transform;
    }

    public void HideLockOnIcon()
    {
        _lockOnIcon.SetActive(false);
    }
    public Transform ResetCam(Action action)
    {
        OnResetCam.Subscribe(_=> action?.Invoke()).AddTo(this);
        return _camPoint; 
    }
}
