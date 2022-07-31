using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Rigidbody))]

//プレイヤーの動きを制御する
public class PlayerMoveController : ColliderGenerater
{
    public static PlayerMoveController Instance { get; private set; }

    [SerializeField]
    [Tooltip("プレイヤーの値の設定")]
    PlayerMoveSettings m_settings = default;

    [SerializeField]
    [Tooltip("接地判定の際、中心 (Pivot) からどれくらいの距離を「接地している」と判定するかの長さ")]
    float m_isGroundedLength = 1.1f;

    [SerializeField]
    [Tooltip("スピードアップエフェクト")]
    GameObject m_speedup = null;

    [SerializeField]
    [Tooltip("ラッシュエフェクト")]
    GameObject m_rush = null;

    [SerializeField]
    [Tooltip("当たるレイヤー")]
    LayerMask _layerMask = 0;

    [SerializeField]
    [Tooltip("スタンス値のSlider")]
    Image m_slider = default;

    BufferParameter _moveSpeed;
    BufferParameter _jumpPower;
    BufferParameter _dodgeDistance;
    BufferParameter _attackSpeed;

    bool IsDodgeing = false;
    float stanceValue;
    const float attackSpeed = 1;
    const float disableMoveTime = 1f;
    /// <summary>浮く距離</summary>
    const float floatUpDistance = 2f;
    /// <summary>浮く時間</summary>
    const float floatUpDuraration = 0.5f;
    Animator _anim = null;
    Rigidbody _rb;
    Vector3 dir;
    Vector3 velo;
    Vector3 latestPos;
    PlayerMoveSettings m_current;

    /// <summary>照準</summary>
    RectTransform m_crosshairUi = null;
    Ray ray;
    RaycastHit hit;

    bool m_isMoveActive = true;

    public float StanceValue => stanceValue;
    public void SetMoveActive(bool IsMoveActive) { m_isMoveActive = IsMoveActive; }

    public void SetActiveMove(int IsMoveActive) { m_isMoveActive = IsMoveActive == 1; }

    #region AnimationHash

    static readonly int SpeedHash = Animator.StringToHash("Speed");

    static readonly int JumpHash = Animator.StringToHash("JumpFlag");

    static readonly int DodgeHash = Animator.StringToHash("CrouchFlag");

    static readonly int BasicAttackHash = Animator.StringToHash("Basic");

    static readonly int JumpAttackHash = Animator.StringToHash("JumpAttack");

    static readonly int JumpPowerAttackHash = Animator.StringToHash("JumpPowerAttack");

    static readonly int ComboHash = Animator.StringToHash("Combo");

    static readonly int IsGroundedHash = Animator.StringToHash("IsGround");

    static readonly int IsAttackEndHash = Animator.StringToHash("IsAttackEnd");
    #endregion

    Coroutine current;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        stanceValue = 0.5f;
        TryGetComponent(out _anim);
        TryGetComponent(out _rb);
        _moveSpeed = new BufferParameter(m_settings.MovingSpeed);
        _jumpPower = new BufferParameter(m_settings.JumpPower);
        _dodgeDistance = new BufferParameter(m_settings.DodgeLength);
        _attackSpeed = new BufferParameter(_anim.speed);
    }

    void Update()
    {
        MoveAction();
        IsGrounded();
        SetStance();
        _anim.speed = _attackSpeed.Value;
    }

    public void Move(Vector3 dir)
    {
        if (IsDodgeing) return;
        if (!m_isMoveActive) dir = Vector3.zero;
        dir = Camera.main.transform.TransformDirection(dir);    // メインカメラを基準に入力方向のベクトルを変換する
        dir.y = 0;  // y 軸方向はゼロにして水平方向のベクトルにする
        SetPlayerAngle(dir);
        Running(dir); // 入力した方向に移動する
        velo.y = _rb.velocity.y;   // ジャンプした時の y 軸方向の速度を保持する
        _rb.velocity = velo;   // 計算した速度ベクトルをセットする
    }

    /// <summary>
    /// エネルギーを増加させる
    /// </summary>
    private void SetStance()
    {
        if(m_slider) stanceValue = m_slider.fillAmount;
    }

    /// <summary>
    /// プレイヤーのY軸角度を変更させる
    /// </summary>
    /// <param name="dir">回転目標方向</param>
    private void SetPlayerAngle(Vector3 dir)
    {
        if (dir == Vector3.zero) return;
        Vector3 diff = transform.position - latestPos;   //前回からどこに進んだかをベクトルで取得
        diff.y = 0;
        latestPos = transform.position;  //前回のPositionの更新

        //ベクトルの大きさが0.01以上の時に向きを変える処理をする
        if (diff.magnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(dir);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, Time.deltaTime * m_settings.TurnSpeed);  // 徐々にプレイヤーを回転させる
        }
    }

    private void MoveAction()
    {
        //空中での入力
        if (!IsGrounded())
        {
            _anim.SetFloat(SpeedHash, 0);
            float veloY = _rb.velocity.y;
            _rb.velocity = new Vector3(_rb.velocity.x, veloY, _rb.velocity.z);
        }
    }
    /// <summary>
    /// ジャンプの挙動を制御する
    /// </summary>
    public void Jump()
    {
        if (!IsGrounded()) return;
        _anim.SetTrigger(JumpHash);
        _rb.useGravity = false;
        _rb.DOMoveY(_jumpPower.Value, 0.2f);
        _rb.constraints = RigidbodyConstraints.FreezeRotation;
        _rb.useGravity = true;
    }

    /// <summary>
    /// 回避時の挙動を制御する
    /// </summary>
    /// <param name="direction">回避方向</param>
    public void Dodge(Vector3 direction)
    {
        if (CanUse && IsGrounded())
        {
            ///回避コルーチンを開始
            StartCoroutine(SetDodge(m_settings.DodgeCoolDown,direction));
        }
    }

    /// <summary>
    /// スキルの発動に必要なエネルギーを追加する
    /// </summary>
    /// <param name="value">エネルギーの回収率(0~1)</param>
    public void AddStanceValue(float value)
    {
        if (value + stanceValue < 1) stanceValue += value;
        else stanceValue = 1f;
        m_slider.fillAmount = stanceValue;
    }

    [Tooltip("行動出来るかどうか")]
    bool CanUse = true;
    /// <summary>
    ///　行動のクールダウンを制御する
    /// </summary>
    /// <param name="cooldown">クールダウン時間</param>
    /// <returns></returns>
    IEnumerator SetDodge(float cooldown, Vector3 direction)
    {
        StartCoroutine(DisableMove());
        RaycastHit hit;
        var dodgeDistance = _dodgeDistance.Value;
        direction = direction.magnitude < 0.2f ? transform.forward * -1 : direction;
        Ray ray = new Ray(transform.position, direction);
        bool Ishit = Physics.Raycast(ray, out hit,_dodgeDistance.Value,_layerMask);
        var hitBox = GetComponent<CapsuleCollider>();
        if (Ishit)
        {
            dodgeDistance = Vector3.Distance(transform.position, hit.point) - hitBox.radius * 2 < dodgeDistance ?
                Vector3.Distance(transform.position, hit.point) - hitBox.radius * 2 : dodgeDistance;
        }
        //m_rush.SetActive(true);
        _anim.Play("Crouch");
        //Debug.Log(direction);
        transform.rotation = Quaternion.LookRotation(direction * -1);
        ///回避時の移動入力に応じて移動距離を変更
        _rb.DOMove(transform.position - direction * dodgeDistance, disableMoveTime);
        IsDodgeing = false;
        CanUse = false;
        yield return new WaitForSeconds(cooldown);
        CanUse = true;
    }

    IEnumerator DisableMove()
    {
        IsDodgeing = true;
        yield return new WaitForSeconds(disableMoveTime);
        IsDodgeing = false;
    }
    /// <summary>
    /// 地面に接触しているか判定する
    /// </summary>
    /// <returns></returns>
    bool IsGrounded()
    {
        // Physics.Linecast() を使って足元から線を張り、そこに何かが衝突していたら true とする
        Vector3 start = this.transform.position;   // start: オブジェクトの中心
        Vector3 end = start + Vector3.down * m_isGroundedLength;  // end: start から真下の地点
        Debug.DrawLine(start, end); // 動作確認用に Scene ウィンドウ上で線を表示する
        bool isGrounded = Physics.Linecast(start, end); // 引いたラインに何かがぶつかっていたら true とする
        _anim.SetBool(IsGroundedHash, isGrounded);
        PlayerManager.Instance.PlayerState = isGrounded ? PlayerState.OnField : PlayerState.InAir;
        return isGrounded;
    }

    bool IsRunning = false;
    /// <summary>
    /// 移動状態を制御する
    /// </summary>
    public void Running(Vector3 dir)
    {
        //入力に応じてスピード、アニメーションを変更する
        velo = dir.normalized * (IsRunning ? m_settings.RunningCorrection : 1f) * _moveSpeed.Value;
        _anim.SetFloat(SpeedHash, (dir == Vector3.zero ? 0 : IsRunning ? m_settings.RunningCorrection * _moveSpeed.Value : _moveSpeed.Value));
    }

    /// <summary>
    /// 走りの状態を切り替える
    /// </summary>
    /// <param name="value">走り入力がされているかどうか</param>
    public void SetRunning(bool value) 
    {
        IsRunning = value;
        m_speedup.SetActive(value);
    }
    public void ChangeMoveSpeed(float value, float time)
    {
        StartCoroutine(_moveSpeed.ChangeValue(value,time));
    }

    public void ChangeDodgeDistance(float value, float time)
    {
        StartCoroutine(_dodgeDistance.ChangeValue(value,time));
    }

    public void ChangeAttackspeedRate(float value, float time)
    {
        StartCoroutine(_attackSpeed.ChangeValue(value, time));
    }

    #region AnimationEvent

    /// <summary>
    /// 攻撃を受けたときのノックバック関数
    /// </summary>
    public void BasicHitAttack() => _rb.DOMove(this.transform.position - Camera.main.transform.forward * 2, 1f);

    /// <summary>
    /// 攻撃を受けたときのノックバック関数
    /// </summary>
    public void SpecialHitAttack() => _rb.DOMove(this.transform.position - Camera.main.transform.forward * 5 + Vector3.up, 0.2f);

    /// <summary>
    /// 剣の軌跡を有効化する
    /// </summary>
    public void StartEmit() => GetComponentInChildren<TrailRenderer>().emitting = true;

    /// <summary>
    /// 剣の軌跡を無効化する
    /// </summary>
    public void StopEmit() => GetComponentInChildren<TrailRenderer>().emitting = false;

    /// <summary>
    /// 重力有効化
    /// </summary>
    public void StopFloat() => _rb.useGravity = true;

    /// <summary>
    /// 重力無効化
    /// </summary>
    public void StartFloat()
    {
        _rb.constraints = RigidbodyConstraints.FreezeAll;
        _rb.DOMoveY(GameManager.Player.transform.position.y + floatUpDistance, floatUpDuraration)
            .OnComplete(() => _rb.constraints = RigidbodyConstraints.FreezeRotation);
    }

    /// <summary>
    /// 回避音を鳴らす
    /// </summary>
    public void PlayDodgeSE() => SoundManager.Instance.PlayDodge();
    #endregion
}
