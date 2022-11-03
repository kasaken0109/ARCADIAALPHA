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
    public static new PlayerMoveController Instance { get; private set; }

    [SerializeField]
    [Tooltip("プレイヤーの値の設定")]
    PlayerMoveSettings m_settings = default;

    [SerializeField]
    [Tooltip("接地判定の際、中心 (Pivot) からどれくらいの距離を「接地している」と判定するかの長さ")]
    float m_isGroundedLength = 1.1f;

    [SerializeField]
    [Tooltip("接地判定の際、中心 (Pivot) からどれくらいの距離を「接地している」と判定するかの長さ")]
    float m_isPreGroundedLength = 1.1f;

    [SerializeField]
    [Tooltip("スピードアップエフェクト")]
    GameObject m_speedup = null;

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
    const float disableMoveTime = 0.5f;
    /// <summary>浮く距離</summary>
    const float floatUpDistance = 2f;
    /// <summary>浮く時間</summary>
    const float floatUpDuraration = 0.5f;
    Animator _anim = null;
    Rigidbody _rb;
    Vector3 velo;
    Vector3 latestPos;

    bool m_isMoveActive = true;

    int dodgeCount = 2;

    PlayerAttackController _attackController;
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
        _attackController = FindObjectOfType<PlayerAttackController>();
        dodge = dodgeCount;
    }

    void Update()
    {
        //MoveAction();
        IsGrounded();
        SetStance();
        _anim.speed = _attackSpeed.Value;
    }

    public void Move(Vector3 dir)
    {
        if (IsDodgeing) return;
        if (!IsGrounded()) return;
        if (!m_isMoveActive) dir = Vector3.zero;
        dir = Camera.main.transform.TransformDirection(dir);    // メインカメラを基準に入力方向のベクトルを変換する
        dir.y = 0;  // y 軸方向はゼロにして水平方向のベクトルにする
        if(!IsDodgeing)SetPlayerAngle(dir);
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
    bool CanJump = true;

    const float coolTime = 0.35f;

    Coroutine jump = null;
    /// <summary>
    /// ジャンプの挙動を制御する
    /// </summary>
    public void Jump()
    {
        if (jump != null) return;
        if (!CanJump)
        {
            jump = StartCoroutine(CooldownSkill());
            return;
        }

        _anim.SetTrigger(JumpHash);
        _anim.SetBool("IsFall", false);
        _rb.DOMoveY(_jumpPower.Value, 0.2f).OnComplete(() =>
        {
            _anim.SetBool("IsFall", true);
            _attackController.CheckPlayerState();
        });

        _rb.constraints = RigidbodyConstraints.FreezeRotation;
        CanJump = false;
    }

    bool IsSetGravity = true;
    public void SetGravity()
    {
        IsSetGravity = false;
    }
    public void RemoveGravity()
    {
        IsSetGravity = true;
        StartCoroutine(Set());
    }

    float time;
    IEnumerator Set()
    {
        while (time < 0.2f)
        {
            _rb.velocity = new Vector3(_rb.velocity.x, -0.1f, _rb.velocity.z);
            time += Time.deltaTime;
            yield return null; 
        }
    }

    IEnumerator CooldownSkill()
    {
        yield return new WaitForSeconds(coolTime);
        CanJump = true;
        jump = null;
    }

    /// <summary>
    /// 回避時の挙動を制御する
    /// </summary>
    /// <param name="direction">回避方向</param>
    public void Dodge(Vector3 direction)
    {
        if (!IsGrounded()) return;
        if (dodge > 0)
        {
            StartCoroutine(DisableMove());
            RaycastHit hit;
            var dodgeDistance = _dodgeDistance.Value;
            var dir = Camera.main.transform.TransformDirection(direction);    // メインカメラを基準に入力方向のベクトルを変換する
            dir.y = 0;
            direction = direction.magnitude < 0.2f ? transform.forward : dir;
            Ray ray = new Ray(transform.position, direction);
            bool Ishit = //Physics.BoxCast(transform.position, Vector3.one * _dodgeDistance.Value, direction, out hit, Quaternion.identity,_dodgeDistance.Value, _layerMask);
            Physics.Raycast(ray, out hit, _dodgeDistance.Value, _layerMask);
            var hitBox = GetComponent<CapsuleCollider>();
            if (Ishit)
            {
                dodgeDistance = Vector3.Distance(transform.position, hit.point) - hitBox.radius * 2 < dodgeDistance ?
                    (Vector3.Distance(transform.position, hit.point) - hitBox.radius * 2) : dodgeDistance;
            }
            _anim.Play("Dodge");
            m_speedup.SetActive(true);
            ///回避時の移動入力に応じて移動距離を変更
            
            _rb.DOMove(transform.position + direction * dodgeDistance, disableMoveTime).OnComplete(() => IsDodgeing = false).SetDelay(0.2f);
            dodge--;
        }
        else
        {
            ///回避コルーチンを開始
            StartCoroutine(SetDodge(m_settings.DodgeCoolDown));
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
    bool canUse = true;
    int dodge;
    /// <summary>
    ///　行動のクールダウンを制御する
    /// </summary>
    /// <param name="cooldown">クールダウン時間</param>
    /// <returns></returns>
    IEnumerator SetDodge(float cooldown)
    {
        yield return new WaitForSeconds(cooldown + 0.5f);
        dodge = dodgeCount;
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
    bool isRunning = false;
    float runSpeedFixer = 0f;
    Coroutine running = null;
    /// <summary>
    /// 移動状態を制御する
    /// </summary>
    public void Running(Vector3 dir)
    {
        isRunning = dir.magnitude >= 0.1f;
        if (isRunning && isEndCoroutine && runSpeedFixer <= 0.9f)
        {
            isEndCoroutine = false;
            StartCoroutine(SetSpeed());
        }
        else if (!isRunning)
        {
            runSpeedFixer = 0;
        }
        //入力に応じてスピード、アニメーションを変更する
        velo = dir.normalized * _moveSpeed.Value * runSpeedFixer;
        _anim.SetFloat(SpeedHash, (dir == Vector3.zero ? 0 : _moveSpeed.Value));
    }

    bool isEndCoroutine = true;
    IEnumerator SetSpeed()
    {
        runSpeedFixer = 0;
        while (runSpeedFixer <= 1f)
        {
            if (!isRunning)
            {
                runSpeedFixer = 0;
                isEndCoroutine = true;
                yield break;
            }
            yield return null;
            runSpeedFixer += 0.01f;
        }
        isEndCoroutine = true;
    }

    /// <summary>
    /// 走りの状態を切り替える
    /// </summary>
    /// <param name="value">走り入力がされているかどうか</param>
    public void SetRunning(bool value) 
    {
        isRunning = value;
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
    public void BasicHitAttack() => _rb.DOMove(this.transform.position - Camera.main.transform.forward * 2, 0.5f)
        .OnStart(() => _rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY)
        .OnComplete(() => _rb.constraints = RigidbodyConstraints.FreezeRotation);

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
    public void StopFloat() => _rb.DOMoveY(0, floatUpDuraration);

    /// <summary>
    /// 重力無効化
    /// </summary>
    public void StartFloat()
    {
        _rb.constraints = RigidbodyConstraints.FreezeAll;
        _rb.DOMoveY(GameManager.Player.transform.position.y , floatUpDuraration)
            .OnComplete(() => _rb.constraints = RigidbodyConstraints.FreezeRotation);
    }

    /// <summary>
    /// 回避音を鳴らす
    /// </summary>
    public void PlayDodgeSE() => SoundManager.Instance.PlayDodge();
    #endregion

    void OnDrawGizmos()
    {
        //　Cubeのレイを疑似的に視覚化
        Gizmos.color = Color.green;
        //Gizmos.DrawWireCube(transform.position + transform.forward * distanceFromTargetObj, Vector3.one);
    }
}
