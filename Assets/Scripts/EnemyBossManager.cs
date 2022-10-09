using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using BehaviourAI;

/// <summary>
/// 敵の状態を管理する
/// </summary>
public class EnemyBossManager : MonoBehaviour, IDamage
{
    public static EnemyBossManager Instance { get; private set; }

    [SerializeField]
    [Tooltip("HP")]
    int m_hp = 100;

    [SerializeField]
    [Tooltip("怯み値")]
    [Range(1, 100)]
    int m_rate;

    [SerializeField]
    [Tooltip("スタン耐久値")]
    int m_stun = 5;

    [SerializeField]
    [Tooltip("スタン時間")]
    float m_stunTime = 8f;

    [SerializeField]
    [Tooltip("毒状態に発生するエフェクト")]
    GameObject _poison = default;

    [SerializeField]
    [Tooltip("Animator")]
    Animator m_animator = null;

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

    bool IsCritical = false;//特殊攻撃のいフラグ

    int maxHp;
    int hitRate = 0;//怯み値
    int rateTemp;
    int count = 0;//特殊攻撃の回数
    int stun;
    float hitstopRate = 0.5f;
    float hitSpeed = 1f;//ヒットストップのスピード
    const float coefficient = 8f;
    const float actionHpRateHalf = 0.5f;
    const float actionHpRateLittle = 0.2f;

    GameObject me;

    Cinemachine.CinemachineImpulseSource impulseSource;

    #region EnemyAnimatorHash
    int hpHash = Animator.StringToHash("HP");
    int mpHash = Animator.StringToHash("MP");

    #endregion

    void Awake()
    {
        Instance = this;
        maxHp = m_hp;
        TryGetComponent(out impulseSource);
        me = gameObject;
        stun = m_stun;
    }

    public void AddDamage(int damage,ref GameObject call)
    {
        if (call.CompareTag("Player"))
        {
            var weaponAttribute = call.GetComponent<WeaponAttributeController>();
            StunChecker(weaponAttribute.StunPower);
            call.GetComponentInParent<PlayerMoveController>().AddStanceValue(weaponAttribute.MPAbsorbValue);
        }
        StopCoroutine(HitStop());
        if (damage >= Mathf.CeilToInt(m_rate * hitstopRate))
        {
            StartCoroutine(HitStop());
            MotorShaker.Instance.Call(ShakeType.Hit, coefficient * damage / maxHp);
        }
        
        if (m_hp < maxHp * actionHpRateHalf && count == 0)
        {
            StartCoroutine(nameof(DeathCombo));
        }
        else if (m_hp < maxHp * actionHpRateLittle && count == 1)
        {
            StopCoroutine(nameof(DeathCombo));
            StartCoroutine(nameof(DeathCombo));
        }
        if (m_hp > damage)
        {
            m_hp -= damage;
            DOTween.To(
                () => hpSlider.fillAmount, // getter
                x => hpSlider.fillAmount = x, // setter
                (float)(float)m_hp / maxHp, // ターゲットとなる値
                1f  // 時間（秒）
                ).SetEase(Ease.OutCubic);
            hitRate += damage;
            if(hitRate >= m_rate)
            {
                m_animator.SetInteger(hpHash, 1);
                m_animator.SetTrigger("Hit");
                hitRate = 0;
            }
        }
        else
        {
            m_hp = 0;
            StopCoroutine(HitStop());
            Time.timeScale = 1;
            DOTween.To(
                () => hpSlider.fillAmount, // getter
                x => hpSlider.fillAmount = x, // setter
                (float)(float)0, // ターゲットとなる値
                1f  // 時間（秒）
                ).SetEase(Ease.OutCubic);
            Instantiate(m_deathBody,this.transform.position,this.transform.rotation);
            FindObjectOfType<CameraController>().RetargetTargetCam();
            GameManager.Instance.SetGameState(GameState.PLAYERWIN);
            Destroy(this.gameObject);
        }
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

    IEnumerator DeathCombo()
    {
        IsCritical = true;
        count++;
        rateTemp = hitRate;
        hitRate = -500;
        float distance = Vector3.Distance(GameManager.Player.transform.position, gameObject.transform.position);
        
        int type = distance >= 7 ? 5 : 4;
        m_animator.SetTrigger("DeathAttack");
        m_animator.SetInteger("AttackType", type);
        yield return new WaitForSeconds(2f);
        IsCritical = false;
        hitRate = rateTemp;
    }

    IEnumerator SlipDamage(int slipValue, float slipInterval,float slipDuraration)
    {
        float time = 0;
        float intervalTime = 0;
        _poison.SetActive(true);
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
        _poison.SetActive(false);

    }

    void StunChecker(int value)
    {
        Debug.Log(stun);
        stun = stun >= value ? stun -= value : 0;
        if (stun == 0)
        {
            Stun();
            stun = m_stun;
        }
    }

    void Stun()
    {
        IEnumerator StunCoroutine()
        {
            m_stunEffect.SetActive(true);
            yield return new WaitForSeconds(m_stunTime);
            m_stunEffect.SetActive(false);
        }
        StartCoroutine(StunCoroutine());
    }

    public void AddSlipDamage(int slipValue, float slipInterval, float slipDuraration)
    {
        StartCoroutine(SlipDamage(slipValue,slipInterval,slipDuraration));
    }

    public void SpawnEffects() => m_sandEffect.SetActive(true);
}
