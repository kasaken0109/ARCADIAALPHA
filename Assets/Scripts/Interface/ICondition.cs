using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 判定条件
/// </summary>
public enum ConditionState
{
    Running,
    Success,
    Failure,
}

/// <summary>
/// 判定の処理
/// </summary>
public interface ICondition
{
    /// <summary>成功したかどうか</summary>
    bool IsSuccess { get; set; }

    /// <summary>判定内容を返す</summary>
    /// <returns>判定結果</returns>
    ConditionState Check();

    /// <summary>
    /// 判定終了時に行う初期化処理
    /// </summary>
    void Reset();
}
