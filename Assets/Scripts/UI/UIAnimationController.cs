using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIAnimationController : MonoBehaviour
{
    [SerializeField]
    float _animTime = 1f;

    bool _isAnimEnd = false;
    RectTransform rectTransform;

    private void OnEnable()
    {
        TryGetComponent(out rectTransform);
        Active();
    }

    public virtual void Active()
    {
        _isAnimEnd = false;
        rectTransform.DOScale(Vector3.one, _animTime).OnComplete(() => _isAnimEnd = true);
    }

    public virtual void NonActive()
    {
        _isAnimEnd = false;
        rectTransform.DOScale(Vector3.zero, _animTime).OnComplete(() => _isAnimEnd = true);
        gameObject.SetActive(false);
    }
}
