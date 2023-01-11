using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// エリアに入ったことを通知する
/// </summary>
public class AreaDetector : MonoBehaviour
{
    [SerializeField]
    [Tooltip("検知する対象のタグ名")]
    string _detectTagName = "Player";

    /// <summary>対象のオブジェクトが入った</summary>
    public Subject<Unit> OnEnterArea = new Subject<Unit>();
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_detectTagName))
        {
            OnEnterArea.OnNext(Unit.Default);
        }
    }
}
