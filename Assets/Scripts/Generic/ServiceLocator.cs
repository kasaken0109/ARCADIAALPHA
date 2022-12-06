using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// サービスロケーター
/// </summary>
public static class ServiceLocator
{
    /// <summary>インスタンスを格納するDictionary </summary>
    static readonly Dictionary<Type, object> _container;

    /// <summary>コンストラクタ</summary>
    static ServiceLocator() => _container = new Dictionary<Type, object>();

    /// <summary>
    /// インスタンスを取得
    /// </summary>
    /// <typeparam name="T">型</typeparam>
    /// <returns>対象のインスタンス</returns>
    public static T GetInstance<T>() => (T)_container[typeof(T)];

    /// <summary>
    /// インスタンスを登録
    /// </summary>
    /// <typeparam name="T">型</typeparam>
    /// <param name="instance">登録するインスタンス</param>
    public static void SetInstance<T>(T instance) => _container[typeof(T)] = instance;

    /// <summary>
    /// インスタンスを登録解除
    /// </summary>
    /// <typeparam name="T">型</typeparam>
    /// <param name="instance">解除するインスタンス</param>
    public static void UnSetInstance<T>(T instance)
    {
        if (Equals(_container[typeof(T)], instance)) _container.Remove(typeof(T));
    }
    /// <summary>
    /// インスタンスを削除
    /// </summary>
    /// <typeparam name="T">削除する型</typeparam>
    public static void RemoveInstance<T>() => _container.Remove(typeof(T));
}
