using System;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Linq;
using UniRx;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("ロックオンカメラ")]
    CinemachineVirtualCamera m_lockOnCamera = default;

    [SerializeField]
    [Tooltip("プレイヤーのカメラ")]
    CinemachineVirtualCamera m_playerCamera = default;

    [SerializeField]
    float _maxLockOnDistance = 5f;

    /// <summary>ロックオン機能の対象</summary>
    private List<Transform> m_lockOnTargets;

    /// <summary>プレイヤー</summary>
    private GameObject m_player;


    /// <summary>カメラ切り替え時に使用するpriority</summary>
    private readonly int cameraPriority = 50;

    /// <summary>現在ロックオンしている敵のID</summary>
    private int lockOnId = 0;

    private CinemachinePOV _cinemachinePOV;

    ReactiveProperty<Transform> _lockOnTarget;
    public Transform OnLockOnTragetchange => _lockOnTarget.Value;

    bool isMonoLoclOn = false;

    bool canLockOn = false;

    public bool IsLockOn => isLockOn;

    bool isLockOn = false;
    bool IsFirstLockOn = false;
    Transform _prevTarget = default;

    private void Awake()
    {
        _lockOnTarget = new ReactiveProperty<Transform>().AddTo(this);
    }
    // Start is called before the first frame update
    void Start()
    {
        CamInit();
        //LockON();
    }

    private void OnEnable()
    {
        CamInit();
        ResetCam();
    }

    /// <summary>
    /// カメラの初期化を行う
    /// </summary>
    private void CamInit()
    {
        m_player = GameObject.FindGameObjectWithTag("Player");
        isLockOn = false;
        m_lockOnTargets = SetSearchTarget<ILockOnTargetable>("Enemy");
        _cinemachinePOV = m_playerCamera.GetCinemachineComponent<CinemachinePOV>();
        lockOnId = 0;
    }

    /// <summary>
    /// ロックオン機能終了時にカメラリセットを行う
    /// </summary>
    private void ResetCam()
    {
        if (_prevTarget != null && canLockOn)
        {
            //m_playerCamera.transform.LookAt(_prevTarget);
            m_playerCamera.ForceCameraPosition((_prevTarget.position + m_playerCamera.transform.position) * 0.5f, Quaternion.LookRotation(_prevTarget.position - m_playerCamera.transform.position));
            return;
        }
        m_lockOnCamera.Priority = 0;
        _cinemachinePOV.m_VerticalAxis.Value = 0;
        _cinemachinePOV.m_HorizontalAxis.Value = -180;
    }

    public float PlanePointAngleDeg(Vector3 from ,Vector3 to,Vector3 planeNormal)
    {
        var fromPlane = Vector3.ProjectOnPlane(from, planeNormal);
        var toPlane = Vector3.ProjectOnPlane(to, planeNormal);
        return Vector3.SignedAngle(fromPlane, toPlane, planeNormal) * Mathf.Rad2Deg;
    }

    /// <summary>
    /// ロックオン機能を終了する
    /// </summary>
    private void LockOff()
    {
        if (m_lockOnTargets.Count > 1 && m_lockOnTargets[lockOnId])
        {
            m_playerCamera.transform.LookAt(m_lockOnTargets[lockOnId]);
            m_lockOnTargets[lockOnId].GetComponentInParent<ILockOnTargetable>().HideLockOnIcon();
        }
        ResetCam();
        Invoke(nameof(ResetCamData), 0f);
    }

    void ResetCamData()
    {
        m_lockOnCamera.Priority = 0;
        lockOnId = 0;
        if (m_lockOnTargets.Count > 0) m_lockOnTargets.Clear();
        m_lockOnTargets = SetSearchTarget<ILockOnTargetable>("Enemy");
    }

    /// <summary>
    /// ロックオン機能を起動、起動中は対象を切り替える
    /// </summary>
    public void LockON()
    {
        var target = SetSearchTarget<ILockOnTargetable>("Enemy");
        if (target.Count() > 0)
        {
            canLockOn = true;
        }
        else return;
        if (target.Count() == 1)
        {
            isMonoLoclOn = true; 
            _prevTarget = target.First().transform;
            _prevTarget = target.First().GetComponentInParent<ILockOnTargetable>().ResetCam(() =>
            {
                canLockOn = false;
                ResetCam();
            });
            if (isLockOn)
            {
                m_lockOnCamera.Priority = 0;
                m_lockOnTargets[0].GetComponentInParent<ILockOnTargetable>().HideLockOnIcon();
                ResetCam();
            }
            else
            {
                m_lockOnTargets.Clear();
                m_lockOnTargets.Add(target.First().transform);
                m_lockOnCamera.LookAt = m_lockOnTargets[0];
                _lockOnTarget.Value = m_lockOnTargets[0];
                m_lockOnTargets[0].GetComponentInParent<ILockOnTargetable>().ShowLockOnIcon();
                m_lockOnCamera.Priority = cameraPriority;
            }
            isLockOn = !isLockOn;
            return;
        }
        if (!isLockOn)
        {
            m_lockOnTargets = SetSearchTarget<ILockOnTargetable>("Enemy");
            LockOnCamera();
        }
        else LockOff();
        isLockOn = !isLockOn;
    }

    private void LockOnCamera()
    {
        if(m_lockOnTargets.Count == 0)
        {
            LockOff();
            return;
        }
        if (!m_lockOnTargets[lockOnId])
        {
            SwitchTarget(true);
            return;
        }
        _prevTarget = m_lockOnTargets[lockOnId].GetComponentInParent<ILockOnTargetable>().ResetCam(() =>
        {
            m_lockOnTargets = SetSearchTarget<ILockOnTargetable>("Enemy");
            if(m_lockOnTargets.Count == 0)
            {
                canLockOn = false;
                ResetCam();
                return;
            }
            lockOnId = 0;
            LockOnCamera();
        });
        m_lockOnCamera.LookAt = m_lockOnTargets[lockOnId];
        _lockOnTarget.Value = m_lockOnCamera.LookAt;
        m_lockOnTargets[lockOnId].GetComponentInParent<ILockOnTargetable>().ShowLockOnIcon();
        m_lockOnCamera.Priority = cameraPriority;
    }

    public void SwitchTarget(bool isPrev)
    {
        if (!isLockOn || isMonoLoclOn || !canLockOn) return;
        if (SetSearchTarget<ILockOnTargetable>("Enemy").Count == 0) LockOff();
        if (m_lockOnTargets.Count != 0 &&  m_lockOnTargets.Count - 1 >= lockOnId)
        {
            m_lockOnTargets[lockOnId].GetComponentInParent<ILockOnTargetable>().HideLockOnIcon();
        }
        else lockOnId = 0;
        m_lockOnTargets = SetSearchTarget<ILockOnTargetable>("Enemy");
        lockOnId = isPrev ? 
            lockOnId == 0 ? m_lockOnTargets.Count - 1: --lockOnId
            :lockOnId == m_lockOnTargets.Count - 1 ? 0 : ++lockOnId;
        LockOnCamera();
    }

    /// <summary>
    /// 対象のタグを持つルートオブジェクトを検索する
    /// </summary>
    /// <param name="tag">対象タグ名</param>
    /// <returns>ロックオン対象のオブジェクト(プレイヤーとの距離が近い順)</returns>
    public List<Transform> SetSearchTarget<T>(string lockOnTag) where T : ILockOnTargetable
    {
        //if (m_lockOnTargets.Count > 0) m_lockOnTargets.Clear();
        var target = GameObject.FindGameObjectsWithTag(lockOnTag);
        return target.Where(c => c.GetComponent<ILockOnTargetable>() != null && Vector3.Distance(c.transform.position,transform.position) <= _maxLockOnDistance).Select(c => c.GetComponent<ILockOnTargetable>().GetCamPoint()).OrderBy(t => Vector3.Distance(m_player.transform.position, t.position)).ToList();//OrderBy(t => Vector3.Distance(m_player.transform.position,t.gameObject.transform.position)).ToList();
    }
}
