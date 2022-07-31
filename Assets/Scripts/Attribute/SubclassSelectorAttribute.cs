using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// SerializeReferenceの項目を表示してくれるEditor拡張
/// </summary>
/// 
[AttributeUsage(AttributeTargets.Field,AllowMultiple = false)]
public class SubclassSelectorAttribute : PropertyAttribute
{
    bool m_includeMono;

    public SubclassSelectorAttribute(bool includeMono = false)
    {
        m_includeMono = includeMono;
    }

    public bool IsIncludeMono() => m_includeMono;
}
