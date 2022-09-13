using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public sealed class PanelAnimationController : UIAnimationController
{
    [SerializeField]
    RectTransform _targetPos;
    [SerializeField]
    RectTransform _stopPos;

    Vector3 originScale;
    Vector3 originPosition;
    RectTransform rectTransform;
    bool IsReachDestination = false;
    bool IsHold = false;
    Tweener start = null;
    Tweener end = null;
    void Start()
    {
        TryGetComponent(out rectTransform);
        originPosition = _stopPos.position;
    }
    public void FadePanel()
    {
        transform.DOMove(_targetPos.position, 1);
    }

    public void DisplayPanelButtonHold()
    {
        IsHold = true;
        end.Kill();
        //Debug.Log($"{IsReachDestination},{IsHold}");
        if(!IsReachDestination && IsHold)
        {
            start = rectTransform.DOMove(originPosition, 0.5f).OnComplete(() => IsReachDestination = true);
            start.Play();
        }
    }

    public void HidePanal()
    {
        IsHold = false;
        start.Kill();
        IsReachDestination = false;
        end = rectTransform.DOMove(_targetPos.position, 0.5f);
        end.Play();
    }

    public void ButtonHold()
    {
        IsHold = true;
    }

    public override void Active()
    {
        base.Active();
    }

    public override void NonActive()
    {
        FadePanel();
    }
}
