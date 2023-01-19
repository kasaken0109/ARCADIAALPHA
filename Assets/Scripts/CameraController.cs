﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Linq;

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
    private readonly int cameraPriority = 15;

    /// <summary>現在ロックオンしている敵のID</summary>
    private int lockOnId = 0;

    private CinemachinePOV _cinemachinePOV;


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
        m_lockOnTargets = SetSearchTarget<ILockOnTargetable>("Enemy");
        _cinemachinePOV = m_playerCamera.GetCinemachineComponent<CinemachinePOV>();
        lockOnId = 0;
    }

    /// <summary>
    /// ロックオン機能終了時にカメラリセットを行う
    /// </summary>
    private void ResetCam()
    {
        _cinemachinePOV.m_VerticalAxis.Value = 0;
        _cinemachinePOV.m_HorizontalAxis.Value = -180;
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
        m_playerCamera.GetCinemachineComponent<CinemachinePOV>().GetRecenterTarget();
        ResetCam();
        m_lockOnCamera.Priority = 0;
        lockOnId = 0;
        if (m_lockOnTargets.Count > 0) m_lockOnTargets.Clear();
        m_lockOnTargets = SetSearchTarget<ILockOnTargetable>("Enemy");
    }

    bool IsLockOn;

    /// <summary>
    /// ロックオン機能を起動、起動中は対象を切り替える
    /// </summary>
    public void LockON()
    {
        if (!IsLockOn)
        {
            m_lockOnTargets = SetSearchTarget<ILockOnTargetable>("Enemy");
            //if (m_lockOnTargets.Count == 0)
            //{
            //    m_lockOnTargets[lockOnId].GetComponentInParent<ILockOnTargetable>().CamReset(RetargetTargetCam);
            //    return;
            //}
            //lockOnId = lockOnId == m_lockOnTargets.Length - 1 ? 0 : lockOnId + 1;
            LockOnCamera();
        }
        else LockOff();
        IsLockOn = !IsLockOn;
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
        m_lockOnTargets[lockOnId].GetComponentInParent<ILockOnTargetable>().ResetCam(() =>
        {
            m_lockOnTargets = SetSearchTarget<ILockOnTargetable>("Enemy");
            lockOnId = 0;
            LockOnCamera();
            //SwitchTarget(true);
        });
        m_lockOnCamera.LookAt = m_lockOnTargets[lockOnId];
        m_lockOnTargets[lockOnId].GetComponentInParent<ILockOnTargetable>().ShowLockOnIcon();
        m_lockOnCamera.Priority = cameraPriority;
    }

    public void SwitchTarget(bool isPrev)
    {
        if (!IsLockOn || SetSearchTarget<ILockOnTargetable>("Enemy").Count == 1) return;
        if (SetSearchTarget<ILockOnTargetable>("Enemy").Count == 0) LockOff();
        if (m_lockOnTargets[lockOnId])
        {
            Debug.Log(m_lockOnTargets[lockOnId]);
            m_lockOnTargets[lockOnId].GetComponentInParent<ILockOnTargetable>().HideLockOnIcon();
        }
        else lockOnId = 0;
        m_lockOnTargets = SetSearchTarget<ILockOnTargetable>("Enemy");
        lockOnId = isPrev ? 
            lockOnId == 0 ? m_lockOnTargets.Count - 1: --lockOnId
            :lockOnId == m_lockOnTargets.Count - 1 ? 0 : ++lockOnId;
        LockOnCamera();
    }

    ///// <summary>
    ///// 特定のコンポーネントを持つオブジェクを検索する、重いのであまり多用しない
    ///// </summary>
    ///// <typeparam name="T"></typeparam>
    ///// <returns></returns>
    //public GameObject[] SetSearchTarget<T>() where T : Component
    //{
    //    return FindObjectsOfType<GameObject>();
    //}


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
