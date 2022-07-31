using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ”»’èğŒ
/// </summary>
public enum ConditionState
{
    Running,
    Success,
    Failure,
}

/// <summary>
/// ”»’è‚Ìˆ—
/// </summary>
public interface ICondition
{
    /// <summary>¬Œ÷‚µ‚½‚©‚Ç‚¤‚©</summary>
    bool IsSuccess { get; set; }

    /// <summary>”»’è“à—e‚ğ•Ô‚·</summary>
    /// <returns>”»’èŒ‹‰Ê</returns>
    ConditionState Check();

    /// <summary>
    /// ”»’èI—¹‚És‚¤‰Šú‰»ˆ—
    /// </summary>
    void Reset();
}
