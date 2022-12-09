using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;
using UniRx;

/// <summary>
/// 弾の処理タイプ
/// </summary>
public enum BulletType
{
    Lay,//Raycastで当たり判定を検知する
    Physics,//Colliderで当たり判定を検知する
    Skill,//スキルを発動する
}

[Flags]
public enum BulletCustomType
{
    Buff = 1,
    Debuff = 2,
    All = 4
}

/// <summary>
/// 弾の発射時の処理を行う
/// </summary>
public class BulletFire : MonoBehaviour
{
    [SerializeField]
    [Tooltip("弾の発生する地点")]
    Transform _bulletMuzzle = null;

    [SerializeField]
    [Tooltip("パッシブスキルのエフェクトが発生する地点")]
    Transform m_passiveEffectPoint = null;

    [SerializeField]
    [Tooltip("エネルギーの量を表示する")]
    Image m_stance = default;

    [SerializeField]
    [Tooltip("エネルギーの消費量予測線")]
    Image m_line = default;

    [SerializeField]
    [Tooltip("クールダウン表示用UI")]
    EquipBulletDisplay[] _bulletDisplay = default;

    [SerializeField]
    GameObject _diaplayHint = default;
    [SerializeField]
    float _waitUntilHintDisplay = 10f;
    /// <summary>現在のエネルギー値</summary>
    float stanceValue = 0.5f;
    /// <summary>装備中の弾の発射時のエネルギー値</summary>
    float consumeValue;
    /// <summary>弾を発射出来るかどうか</summary>
    bool canShoot = true;
    /// <summary>AddDamageを呼ぶオブジェクト</summary>
    GameObject call = default;
    /// <summary>現在装備中の弾</summary>
    Bullet equip;
    /// <summary>クールダウン値(可変)</summary>
    BufferParameter cooldownTime;
    /// <summary>弾のダメージ(可変)</summary>
    BufferParameter bulletDamage;
    /// <summary>弾の消費エネルギー(可変)</summary>
    BufferParameter bulletEnergy;

    /// <summary>エネルギーの数値</summary>
    readonly ReactiveProperty<float> _ep = new ReactiveProperty<float>(0);

    public bool CanFire => canShoot;
    public int CurrentEquipID { set => _currentEquipID = value; }

    int _currentEquipID = 0;

    const float InitialEp = 0.5f;

    float nonInputTimer = 0f;

    bool IsTimerActive = true;

    DroneController droneController;
    void Start()
    {
        _ep.Where(x => x >= 1f).Subscribe(x => TimerUpdate()).AddTo(this);
        Observable.EveryUpdate().Subscribe((x) => 
        {
            _diaplayHint.SetActive(nonInputTimer >= _waitUntilHintDisplay);
            IsTimerActive = _ep.Value >= 1f; 
        }).AddTo(this);
        
        _ep.Subscribe((x) => {
            m_stance.fillAmount = x; 
            if(equip != null) m_line.fillAmount = x - consumeValue;
        }).AddTo(this);
        _ep.Value = InitialEp;
        call = gameObject;
        TryGetComponent(out droneController);
    }

    void TimerUpdate()
    {
        const float WaitTime = 0.5f; 
        IEnumerator Update()
        {
            yield return new WaitForSeconds(WaitTime);
            while (IsTimerActive)
            {
                nonInputTimer += Time.deltaTime;
                yield return null;
            }
            nonInputTimer = 0;
        }
        StartCoroutine(Update());
    }

    /// <summary>
    /// スキルの発動に必要なエネルギーを追加する
    /// </summary>
    /// <param name="value">エネルギーの回収率(0~1)</param>
    public void AddStanceValue(float value)
    {
        if (value + _ep.Value < 1) _ep.Value = _ep.Value + value;
        else _ep.Value = 1f;
    }

    /// <summary>
    /// 弾、スキル弾を発生させる
    /// </summary>
    public void ShootBullet()
    {
        if (GameManager.Instance.GameStatus == GameState.STOP) return;//一時停止中には射撃不可
        if (!canShoot)
        {
            //SoundManager.Instance.PlayEmptyBullet();
            return;
        }
        //SoundManager.Instance.StopSE();

        //銃弾を生成
        if (stanceValue >= equip.ConsumeStanceValue)
        {
            //canShoot = true;
            BulletLaserController laser;
            PlayerBulletController bullet;
            LayserModuleController layserModule;
            var target = GameObject.FindGameObjectsWithTag("Enemy")
                .Single(c => c.GetComponent<HitPosRetention>());
            var instance = equip.MyBullet ? Instantiate(equip.MyBullet, _bulletMuzzle.position, Quaternion.identity) : new GameObject("default");
            var targetpos = new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z);
            instance.transform.LookAt(targetpos);
            droneController.IsShooting = true;
            droneController.LookEnemy(target);
            switch (equip.BulletType)
            {
                case BulletType.Lay:
                    if (instance.TryGetComponent(out laser))
                    {
                        laser.Damage = Mathf.CeilToInt(bulletDamage.Value);
                        laser.Target = target;
                    }
                    else if (instance.TryGetComponent(out layserModule))
                    {
                        layserModule.Damage = Mathf.CeilToInt(bulletDamage.Value);
                        instance.transform.SetParent(transform);
                    }
                    break;
                case BulletType.Physics:
                    if (instance.TryGetComponent(out bullet)) bullet.Damage = Mathf.CeilToInt(bulletDamage.Value);
                    break;
                case BulletType.Skill:
                    equip.PassiveAction?.Execute();
                    if (equip.BulletCustomType == BulletCustomType.Buff) droneController.BuffMovement(equip.AttackDuraration);
                    //バフのエフェクトを生成
                    if (instance)
                    {
                        instance.transform.SetParent(m_passiveEffectPoint);
                        instance.transform.localPosition = Vector3.zero;
                        instance.transform.localRotation = Quaternion.identity;
                        if (instance.TryGetComponent(out laser)) laser.Damage = Mathf.CeilToInt(bulletDamage.Value);
                    }
                    var addtime = equip.PassiveAction != null ? equip.PassiveAction.GetEffectiveTime() : 0;
                    if (!equip.IsPermanence)
                    {
                        Destroy(instance, equip.AttackDuraration + addtime);
                        return;
                    }
                    var effectDisplay = instance.GetComponent<EffectDelayDisplayer>();
                    effectDisplay.DelayDestroy = equip.AttackDuraration + addtime;

                    break;
                default:
                    break;
            }
            if (equip.passiveSkill != null && equip.PassiveSkill.Type == CustomSkill.SkillType.Buf)
            {
                instance.GetComponent<ICustomSkillEvent>().CustomSkillEvent += (x) => equip.PassiveSkill.CustomSkillAction.Execute(x);
            }
            //SoundManager.Instance.PlayShoot();
            _ep.Value = _ep.Value - bulletEnergy.Value;
        }
        else
        {
            //canShoot = false;
            //SoundManager.Instance.PlayEmptyBullet();
        }
        //パッシブ用のコストがある場合パッシブを発動
        StartCoroutine(CoolDown());
    }

    /// <summary>
    /// クールダウンを計測する
    /// </summary>
    /// <returns></returns>
    IEnumerator CoolDown()
    {
        canShoot = false;
        yield return new WaitForSeconds(equip.AttackDuraration);
        droneController.IsShooting = false;
        _bulletDisplay[_currentEquipID].CoolDown(cooldownTime.Value);
        yield return new WaitForSeconds(cooldownTime.Value);
        canShoot = true;
    }

    /// <summary>
    /// 弾を装備する
    /// </summary>
    /// <param name="bullet"></param>
    public void EquipBullet(Bullet bullet) 
    {
        equip = bullet;
        cooldownTime = new BufferParameter(equip.Delay);
        bulletDamage = new BufferParameter(equip.Damage);
        bulletEnergy = new BufferParameter(equip.ConsumeStanceValue);
        var value = equip.PassiveAction != null? equip.PassiveAction.GetEffectiveValue() : 0;
        if(equip.PassiveSkill && equip.PassiveSkill.CustomSkillAction != null && equip.PassiveSkill.Type == CustomSkill.SkillType.Passive) equip.PassiveSkill.CustomSkillAction.Execute(value);

        //パッシブスキルセット時のコストを計算
        consumeValue = equip.ConsumeStanceValue + (equip.PassiveSkill != null ? equip.PassiveSkill.ConsumeCost : 0);
        m_line.fillAmount = _ep.Value - consumeValue;
    }

    /// <summary>
    /// 弾の消費エネルギーを変更する
    /// </summary>
    /// <param name="reduceRate">変更割合(0～1)</param>
    public void ChangeBulletEnergy(float reduceRate)
    {
        StartCoroutine(bulletEnergy.ChangeValue(reduceRate, 0));
    }

    /// <summary>
    /// 弾のダメージを変更する
    /// </summary>
    /// <param name="reduceRate">変更割合(1～)</param>
    public void ChangeBulletDamage(float addRate)
    {
        StartCoroutine(bulletDamage.ChangeValue(addRate, 0));
    }

    /// <summary>
    /// 弾のクールダウンを変更する
    /// </summary>
    /// <param name="reduceRate">変更割合(0～1)</param>
    public void ChangeCoolDown(float reduceRate)
    {
        StartCoroutine(cooldownTime.ChangeValue(reduceRate, 0));
    }

    private void OnDestroy()
    {
        _ep.Dispose();
    }
}
