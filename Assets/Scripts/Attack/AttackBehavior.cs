using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackBehavior :IInputBehavior
{
    [SerializeField]
    [Tooltip("使用するアニメーター")]
    Animator _animator = default;

    [SerializeField]
    [Tooltip("発動するモーション")]
    AttackMotion _attackMotion = default;
    public bool IsEnd => _isEnd;

    bool _isEnd = false;

    float time = 0;

    public void Execute()
    {
        _animator.Play(_attackMotion.AttackClipName);
        _isEnd = false;
        while (_attackMotion.ClipDuraration >= time)
        {
            time += Time.deltaTime;
        }
        _isEnd = true;
    }
}
