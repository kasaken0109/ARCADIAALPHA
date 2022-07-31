using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 攻撃の情報を持つ
/// </summary>
[CreateAssetMenu]
public class AttackData : ScriptableObject
{
    [SerializeField]
    [Tooltip("有効にするコライダーのID")]
    int _activeColliderIndex = 0;

    [SerializeField]
    [Tooltip("有効にする時間")]
    float _activeDuraration = 0.5f;

    [SerializeField]
    [Tooltip("攻撃の補正値")]
    float _attackRate = 1.0f;

    public int ActiveColliderIndex => _activeColliderIndex;
    public float ActiveDuarration => _activeDuraration;
    public float AttackRate => _attackRate;
}
