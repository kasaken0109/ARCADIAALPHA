using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class IndicatorController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("矢印が動く目的地")]
    Vector3 root = default;
    [SerializeField]
    Transform _startPos = default;

    float _lookAngle;
    string _targetName = "Gate";
    RectTransform _rect;
    GameObject _target;
    GameObject _player;
    Sequence move;

    const float moveDuraration = 1f;
    const float xAngle = 90f;

    void OnEnable()
    {
        _target = GameObject.Find(_targetName);
        _player = GameObject.FindGameObjectWithTag("Player");
        TryGetComponent(out _rect);
        StartCoroutine(LookGoal());
        MoveAnim();
    }

    /// <summary>
    /// 矢印をアニメーションさせる
    /// </summary>
    void MoveAnim()
    {
        move = DOTween.Sequence();
        move.Append(_rect.transform.DOLocalMove(_startPos.position + root, moveDuraration))
            //.Append(_rect.transform.DOMove(_player.transform.position, moveDuraration))
            .SetLoops(-1,LoopType.Yoyo);
        move.Play();
    }

    /// <summary>
    /// ゴールの方向にインジケーターを向ける
    /// </summary>
    /// <returns></returns>
    IEnumerator LookGoal()
    {
        while (true)
        {
            if (_target)
            {
                var targetPos = _target.transform.position - transform.position;
                _lookAngle = Mathf.Atan2(targetPos.x, targetPos.z);
                _rect.rotation = Quaternion.Euler(xAngle, _lookAngle, 0);
            }
            yield return null;
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        move.Kill();
    }
}
