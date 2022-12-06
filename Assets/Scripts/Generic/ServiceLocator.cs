using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �T�[�r�X���P�[�^�[
/// </summary>
public static class ServiceLocator
{
    /// <summary>�C���X�^���X���i�[����Dictionary </summary>
    static readonly Dictionary<Type, object> _container;

    /// <summary>�R���X�g���N�^</summary>
    static ServiceLocator() => _container = new Dictionary<Type, object>();

    /// <summary>
    /// �C���X�^���X���擾
    /// </summary>
    /// <typeparam name="T">�^</typeparam>
    /// <returns>�Ώۂ̃C���X�^���X</returns>
    public static T GetInstance<T>() => (T)_container[typeof(T)];

    /// <summary>
    /// �C���X�^���X��o�^
    /// </summary>
    /// <typeparam name="T">�^</typeparam>
    /// <param name="instance">�o�^����C���X�^���X</param>
    public static void SetInstance<T>(T instance) => _container[typeof(T)] = instance;

    /// <summary>
    /// �C���X�^���X��o�^����
    /// </summary>
    /// <typeparam name="T">�^</typeparam>
    /// <param name="instance">��������C���X�^���X</param>
    public static void UnSetInstance<T>(T instance)
    {
        if (Equals(_container[typeof(T)], instance)) _container.Remove(typeof(T));
    }
    /// <summary>
    /// �C���X�^���X���폜
    /// </summary>
    /// <typeparam name="T">�폜����^</typeparam>
    public static void RemoveInstance<T>() => _container.Remove(typeof(T));
}
