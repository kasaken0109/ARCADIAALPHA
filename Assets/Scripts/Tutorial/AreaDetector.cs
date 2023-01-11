using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// �G���A�ɓ��������Ƃ�ʒm����
/// </summary>
public class AreaDetector : MonoBehaviour
{
    [SerializeField]
    [Tooltip("���m����Ώۂ̃^�O��")]
    string _detectTagName = "Player";

    /// <summary>�Ώۂ̃I�u�W�F�N�g��������</summary>
    public Subject<Unit> OnEnterArea = new Subject<Unit>();
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_detectTagName))
        {
            OnEnterArea.OnNext(Unit.Default);
        }
    }
}
