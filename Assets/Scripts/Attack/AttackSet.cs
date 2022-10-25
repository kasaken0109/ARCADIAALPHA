using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 次に再生するステート名と遷移する判定式の情報を持つ
/// </summary>
[System.Serializable]
public class AttackSet
{
    [Tooltip("次のステート名")]
    public string _NextStateName = "";

    [Tooltip("判定失敗時のステート名")]
    public string _FailedStateName = "";

    [SerializeReference,SubclassSelector]
    [Tooltip("判定クラス")]
    public ICondition _condition = default;
}
