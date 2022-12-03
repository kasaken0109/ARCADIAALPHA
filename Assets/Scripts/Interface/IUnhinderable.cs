using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 他のクラスからの操作介入可能判定の定義をする
/// </summary>
public interface IUnhinderable
{
    public bool IsHinderable();
}
