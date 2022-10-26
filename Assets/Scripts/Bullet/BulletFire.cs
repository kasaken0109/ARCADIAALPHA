using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;

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
    Image[] _cooldownUI = default;

    [SerializeField]
    GameObject _cooldownInformation;

    [SerializeField]
    [Tooltip("弾の情報を表示するUI")]
    BulletInformation m_bullet;

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

    /// <summary>現在のクールダウン表示</summary>
    Image currentCoolDown = null;

    DroneController droneController;
    void Start()
    {
        m_stance.fillAmount = 0.5f;
        foreach (var item in _cooldownUI)
        {
            item.fillAmount = 0;
        }
        _cooldownInformation.SetActive(false);
        call = gameObject;
        TryGetComponent(out droneController);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.GameStatus == GameState.STOP) return;//一時停止中には射撃不可
        stanceValue = m_stance.fillAmount;

        if(equip != null)m_line.fillAmount = m_stance.fillAmount - consumeValue;
    }

    /// <summary>
    /// 弾、スキル弾を発生させる
    /// </summary>
    public void ShootBullet()
    {
        if (!canShoot)
        {
            //SoundManager.Instance.PlayEmptyBullet();
            return;
        }
        //SoundManager.Instance.StopSE();

        //銃弾を生成
        if (stanceValue >= equip.ConsumeStanceValue)
        {
            BulletLaserController laser;
            PlayerBulletController bullet;
            LayserModuleController layserModule;
            var target = GameObject.FindGameObjectsWithTag("Enemy")
                .Single(c => c.GetComponent<HitPosRetention>());
            //Debug.Log(target.name);
            var instance = Instantiate(equip.MyBullet, _bulletMuzzle.position, Quaternion.identity);
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
                        //laser.Target = target;
                        instance.transform.SetParent(transform);
                    }
                    break;
                case BulletType.Physics:
                    if (instance.TryGetComponent(out bullet)) bullet.Damage = Mathf.CeilToInt(bulletDamage.Value);
                    break;
                case BulletType.Skill:
                    equip.PassiveAction.Execute();
                    if (instance.TryGetComponent(out laser)) laser.Damage = Mathf.CeilToInt(bulletDamage.Value);
                    FindObjectOfType<PlayerManager>().AddDamage(bulletDamage.Value < 0 ?Mathf.CeilToInt(bulletDamage.Value) : 0, ref call);
                    Destroy(instance, 5);
                    break;
                default:
                    break;
            }
            if (equip.passiveSkill != null && equip.PassiveSkill.Type == CustomSkill.SkillType.Buf) instance.GetComponent<ICustomSkillEvent>().CustomSkillEvent += (x) => equip.PassiveSkill.CustomSkillAction.Execute(x);
            //SoundManager.Instance.PlayShoot();
            stanceValue -= bulletEnergy.Value;
            m_stance.fillAmount = stanceValue;
        }
        //else SoundManager.Instance.PlayEmptyBullet();
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
        _cooldownInformation.SetActive(true);
        _cooldownUI[0].fillAmount = 1;
        yield return null;// new WaitForSeconds(cooldownTime.Value);
        DOTween.To(
                    () => _cooldownUI[0].fillAmount,
                    (x) => _cooldownUI[0].fillAmount = x,
                    0,
                    cooldownTime.Value
                   )
                .OnComplete(() =>
                {
                    _cooldownInformation.SetActive(false);
                    canShoot = true;
                });
        yield return new WaitForSeconds(0.5f);
        droneController.IsShooting = false;

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
        if(equip.PassiveSkill && equip.PassiveSkill.CustomSkillAction != null && equip.PassiveSkill.Type == CustomSkill.SkillType.Passive) equip.PassiveSkill.CustomSkillAction.Execute();
        m_bullet._NameDisplay.sprite= bullet.EquipImage;
        m_bullet._skillDisplay.sprite = bullet.passiveSkill ? bullet.passiveSkill.ImageBullet : null;

        //パッシブスキルセット時のコストを計算
        consumeValue = equip.ConsumeStanceValue + (equip.PassiveSkill != null ? equip.PassiveSkill.ConsumeCost : 0); 
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
}
