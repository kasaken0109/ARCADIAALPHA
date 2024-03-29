using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// パネルの移動処理を行う
/// </summary>
public  class PanelAnimationController : UIAnimationController,IUnhinderable
{
    [SerializeField]
    float _animSpeed = 30f;
    Vector2 originPosition;
    RectTransform _rectTransform;
    bool isReachDestination = false;
    bool isHold = false;
    bool isDisplayChange = false;
    Coroutine startC = null;
    Coroutine endC = null;
    const float threshold = 30f;
    const float displayAnchor = -2797.4f;
    const float leftAnchor = -2102.6f;
    const float rightAnchor = -259.67f;
    void Awake()
    {
        TryGetComponent(out _rectTransform);
        originPosition = _rectTransform.offsetMin;
    }
    public void FadePanel()
    {
        //IEnumerator MovePos()
        //{
        //    isReachDestination = false;
        //    while (Mathf.Abs(_rectTransform.offsetMin.x - rightAnchor) >= threshold)
        //    {
        //        _rectTransform.offsetMin = new Vector2(_rectTransform.offsetMin.x + _animSpeed, originPosition.y);// _rectTransform.offsetMin.x > leftAnchor ? new Vector2(_rectTransform.offsetMin.x - 20f, originPosition.y) : new Vector2(_rectTransform.offsetMin.x + 20f, originPosition.y);
        //        yield return null;
        //    }
        //    isReachDestination = true;
        //}
        //StartCoroutine(MovePos());

    }

    public void BackPanel()
    {
        IEnumerator MovePos()
        {
            isReachDestination = false;
            //Debug.Log(_rectTransform);
            while (Mathf.Abs(_rectTransform.offsetMin.x - displayAnchor) >= threshold)
            {
                _rectTransform.offsetMin = new Vector2(_rectTransform.offsetMin.x - _animSpeed, originPosition.y);// _rectTransform.offsetMin.x > leftAnchor ? new Vector2(_rectTransform.offsetMin.x - 20f, originPosition.y) : new Vector2(_rectTransform.offsetMin.x + 20f, originPosition.y);
                yield return null;
            }
            isReachDestination = true;
        }
        StartCoroutine(MovePos());
    }

    public void DisplayPanelButtonHold()
    {
        isHold = true;
        StopCoroutine(MovePosMinos());
        endC = null;
        if(!isReachDestination && isHold && isDisplayChange)
        {
            startC = StartCoroutine(MovePos());
        }
    }

    public void HidePanal()
    {
        if (!isDisplayChange) return;
           isHold = false;
        StopCoroutine(MovePos());
        startC = null;
        isReachDestination = false;
        endC = StartCoroutine(MovePosMinos());
    }

    IEnumerator MovePos()
    {
        while (Mathf.Abs(_rectTransform.offsetMin.x - leftAnchor) >= threshold)
        {
            if (!isHold) yield break;
            _rectTransform.offsetMin = new Vector2(_rectTransform.offsetMin.x - _animSpeed, originPosition.y);// _rectTransform.offsetMin.x > leftAnchor ? new Vector2(_rectTransform.offsetMin.x - 20f, originPosition.y) : new Vector2(_rectTransform.offsetMin.x + 20f, originPosition.y);
            yield return null;
        }
        isReachDestination = true;

    }

    IEnumerator MovePosMinos()
    {
        while (Mathf.Abs(_rectTransform.offsetMin.x - rightAnchor) >= threshold)
        {
            if (isHold) yield break;
            _rectTransform.offsetMin = new Vector2(_rectTransform.offsetMin.x + _animSpeed, originPosition.y);//_rectTransform.offsetMin.x > rightAnchor ? new Vector2(_rectTransform.offsetMin.x - 20f, originPosition.y) : new Vector2(_rectTransform.offsetMin.x + 20f, originPosition.y);
            yield return null;
        }
    }

    public void SetEnableDisplayChange(bool isActive)
    {
        isDisplayChange = isActive;
    }

    public void ButtonHold()
    {
        isHold = true;
    }

    public override void Active()
    {
        _isAnimEnd = false;
        _rectTransform.DOLocalMove(base._displayPos.transform.localPosition, base._animTime)
            .OnComplete(() => _isAnimEnd = true);
    }

    public override void NonActive()
    {
        _isAnimEnd = false;
        _rectTransform.DOLocalMove(base._hidePos.transform.localPosition, base._animTime)
            .OnComplete(() => _isAnimEnd = true);
    }

    public override bool IsHinderable()
    {
        //return isReachDestination;
        return base._isAnimEnd;
    }
}
