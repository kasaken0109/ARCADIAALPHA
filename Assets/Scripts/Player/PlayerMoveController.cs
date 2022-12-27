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
    GameObject _animObject = default;

    [SerializeField]
    [Tooltip("接地判定の際、中心 (Pivot) からどれくらいの距離を「接地している」と判定するかの長さ")]
    float m_isGroundedLength = 1.1f;

    [SerializeField]
    [Tooltip("スピードアップエフェクト")]
    GameObject m_speedup = null;

    [SerializeField]
    [Tooltip("当たるレイヤー")]
    LayerMask _layerMask = 0;

    [SerializeField]
    [Tooltip("回避ゲージ")]
    Image[] _dodgeGauges;

    bool IsDodgeing = false;
    float stanceValue;
    const float DisableMoveTime = 0.5f;
    /// <summary>浮く距離</summary>
    const float FloatUpDistance = 2f;
    /// <summary>浮く時間</summary>
    const float FloatUpDuraration = 0.5f;
    Animator _anim = null;
    Rigidbody _rb;
    Vector3 velo;
    Vector3 latestPos;

    bool _isMoveActive = true;

    const int dodgeCount = 2;

    PlayerAttackController _attackController;
    public float StanceValue => stanceValue;
    public void SetMoveActive(bool IsMoveActive) { _isMoveActive = IsMoveActive; }

    public void SetMoveActiveDelay(bool IsMoveActive,float delayTime)
    {
        StartCoroutine(Delay());
        IEnumerator Delay()
        {
            yield return new WaitForSeconds(delayTime);
            _isMoveActive = IsMoveActive;
        }
    }

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
        _attackController = FindObjectOfType<PlayerAttackController>();
        dodge = dodgeCount;
    }

    void Update()
    {
        IsGrounded();
    }

    public void Move(Vector3 dir)
    {
        if (IsDodgeing || !IsGrounded()) return;
        if (!_isMoveActive)
        {
            _rb.velocity = Vector3.zero;
            return;
        }
        dir = Camera.main.transform.TransformDirection(dir);    // メインカメラを基準に入力方向のベクトルを変換する
        dir.y = 0;  // y 軸方向はゼロにして水平方向のベクトルにする
        SetPlayerAngle(dir);
        velo = dir.normalized * m_settings.MovingSpeed;
        _anim.SetFloat(SpeedHash, (dir == Vector3.zero ? 0 : m_settings.MovingSpeed)); // 入力した方向に移動する
        velo.y = _rb.velocity.y;   // ジャンプした時の y 軸方向の速度を保持する
        _rb.velocity = velo;   // 計算した速度ベクトルをセットする
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
            _animObject.transform.rotation = Quaternion.Slerp(_animObject.transform.rotation, targetRotation, Time.deltaTime * m_settings.TurnSpeed);  // 徐々にプレイヤーを回転させる
        }
    }
    bool CanJump = true;

    /// <summary>
    /// ジャンプの挙動を制御する
    /// </summary>
    public void Jump()
    {
        if (!CanJump || !IsGrounded()) return;
        _anim.SetTrigger(JumpHash);
        _anim.SetBool("IsFall", false);
        _attackController.CheckPlayerState();
        _rb.DOMoveY(m_settings.JumpPower, 0.2f).OnComplete(() =>
        {
            _anim.SetBool("IsFall", true);
            _attackController.CheckPlayerState();
            StartCoroutine(CooldownSkill());
            CanJump = false;
            _rb.constraints = RigidbodyConstraints.FreezeRotation;
        });
    }
    IEnumerator CooldownSkill()
    {
        yield return new WaitUntil(() => IsGrounded());
        CanJump = true;
    }

    Vector3 VectorFix = new Vector3(0, 0.2f, 0);
    /// <summary>
    /// 回避時の挙動を制御する
    /// </summary>
    /// <param name="direction">回避方向</param>
    public void Dodge(Vector3 direction)
    {
        Debug.Log("Call2");
        if (!IsGrounded() || dodge <= 0 || IsDodgeing) return;

        StartCoroutine(DisableMove());//回避中は自由に移動できないようにする
        var dir = direction.magnitude < 0.2f ? _animObject.transform.forward 
            :　new Vector3(Camera.main.transform.TransformDirection(direction).x,0, Camera.main.transform.TransformDirection(direction).z);
        Ray ray = new Ray(transform.position + VectorFix, dir);
        bool Ishit = Physics.Raycast(ray, out RaycastHit hit, m_settings.DodgeLength, _layerMask);
        Debug.DrawRay(transform.position + VectorFix, dir, Color.red);
        var hitBox = GetComponent<CapsuleCollider>();
        var dodgeDistance = Vector3.Distance(transform.position, hit.point) - hitBox.radius * 2 < m_settings.DodgeLength || Ishit ?
                (Vector3.Distance(transform.position, hit.point) - hitBox.radius) : m_settings.DodgeLength;
        _anim.Play("Dodge");
        m_speedup.SetActive(true);

        _animObject.transform.LookAt(transform.position + dir * dodgeDistance);
        ///回避時の移動入力に応じて移動距離を変更
        _rb.DOMove(transform.position + dir * dodgeDistance, DisableMoveTime)
            .OnStart(() => IsDodgeing = true)
            //.OnComplete(() => IsDodgeing = false)
            .SetDelay(0.2f);

        dodge--;

        _dodgeGauges[dodge].DOFillAmount(0, DisableMoveTime)
            .SetEase(Ease.Linear)
            .OnComplete(() => {
                if (dodge == 0) StartCoroutine(SetDodge(m_settings.DodgeCoolDown));
                else StartCoroutine(Wait());
            });

        IEnumerator Wait()
        {
            yield return new WaitForSeconds(0.5f);
            IsDodgeing = false;
        }
    }
    int dodge;
    /// <summary>
    ///　行動のクールダウンを制御する
    /// </summary>
    /// <param name="cooldown">クールダウン時間</param>
    /// <returns></returns>
    IEnumerator SetDodge(float cooldown)
    {
        _dodgeGauges[0].DOFillAmount(1f, cooldown / 2f).OnComplete(() =>_dodgeGauges[1].DOFillAmount(1f, cooldown / 2f)).SetEase(Ease.Linear);
        yield return new WaitForSeconds(cooldown + 0.2f);
        dodge = dodgeCount;
    }

    const float StopTime = 0.3f;
    IEnumerator DisableMove()
    {
        IsDodgeing = true;
        yield return new WaitForSeconds(DisableMoveTime + StopTime);
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
        //Debug.DrawLine(start, end); // 動作確認用に Scene ウィンドウ上で線を表示する
        bool isGrounded = Physics.Linecast(start, end,out RaycastHit hit,_layerMask); // 引いたラインに何かがぶつかっていたら true とする
        //if (hit.collider != null) Debug.Log(hit.collider.name);
        _anim.SetBool(IsGroundedHash, isGrounded);
        PlayerManager.Instance.PlayerState = isGrounded ? PlayerState.OnField : PlayerState.InAir;
        return isGrounded;
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
    /// 重力有効化
    /// </summary>
    public void StopFloat() => _rb.DOMoveY(0, FloatUpDuraration);

    /// <summary>
    /// 重力無効化
    /// </summary>
    public void StartFloat()
    {
        _rb.constraints = RigidbodyConstraints.FreezeAll;
        _rb.DOMoveY(GameManager.Player.transform.position.y , FloatUpDuraration)
            .OnComplete(() => _rb.constraints = RigidbodyConstraints.FreezeRotation);
    }

    /// <summary>
    /// 回避音を鳴らす
    /// </summary>
    public void PlayDodgeSE() => SoundManager.Instance.PlayDodge();
    #endregion
}
