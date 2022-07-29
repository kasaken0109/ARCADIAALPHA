using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

/// <summary>
/// �v���C���[���S���̃J�����̋����𐧌䂷��
/// </summary>
public class DollyCamController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("�ړ������")]
    float _moveAmount = 2f;

    [SerializeField]
    [Tooltip("�ړ��ɂ����鎞��")]
    float _moveDuraration = 4f;

    CinemachineVirtualCamera _cinemachine;
    CinemachineTrackedDolly _dolly;
    // Start is called before the first frame update
    void Start()
    {
        TryGetComponent(out _cinemachine);
        _dolly = _cinemachine.GetCinemachineComponent<CinemachineTrackedDolly>();
        DOTween.To(() => _dolly.m_PathPosition, (x) => _dolly.m_PathPosition = x, _moveAmount, _moveDuraration);
    }
}
