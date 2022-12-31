using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class DetailWindowController : MonoBehaviour,ISelectHandler,IDeselectHandler
{
    [SerializeField]
    RectTransform _rectTransform;
    [SerializeField]
    float _moveHeight = 60f;

    float _originHeight;
    public void OnDeselect(BaseEventData eventData)
    {
        ActiveWindow(false);
    }

    public void OnSelect(BaseEventData eventData)
    {
        ActiveWindow(true);
    }
    // Start is called before the first frame update
    void Awake()
    {
        _originHeight = _rectTransform.localPosition.y;
        ActiveWindow(false);
    }

    void ActiveWindow(bool IsActive)
    {
        _rectTransform.DOLocalMoveY(_originHeight + _moveHeight *(IsActive ? 0 : -1f),0.5f);
    }
}
