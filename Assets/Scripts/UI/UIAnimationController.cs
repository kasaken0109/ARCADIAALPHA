using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class UIAnimationController : MonoBehaviour,IUnhinderable
{
    [SerializeField]
    protected float _animTime = 1f;
    [SerializeField]
    protected RectTransform _hidePos;
    [SerializeField]
    protected RectTransform _displayPos;

    protected bool _isAnimEnd = false;
    RectTransform rectTransform;

    private void OnEnable()
    {
        TryGetComponent(out rectTransform);
        Active();
    }

    public virtual void Active()
    {
        _isAnimEnd = false;
        rectTransform.DOMove(_displayPos.position, _animTime).OnComplete(() => _isAnimEnd = true);
    }

    public virtual void NonActive()
    {
        _isAnimEnd = false;
        rectTransform.DOMove(_hidePos.position, _animTime).OnComplete(() => {
            _isAnimEnd = true;
            gameObject.SetActive(false);
            });
    }

    public virtual bool IsHinderable()
    {
        return _isAnimEnd;
    }
}
