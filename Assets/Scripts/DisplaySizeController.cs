using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class DisplaySizeController : MonoBehaviour
{
    Image _parentImage;
    RectTransform _parentRect;
    RectTransform _rect;
    ReactiveProperty<float> rectReactive = new ReactiveProperty<float>();

    // Start is called before the first frame update
    void Awake()
    {
        _parentImage = GetComponentInParent<Image>();
        TryGetComponent(out _rect);
        _parentImage.TryGetComponent(out _parentRect);
    }

    private void Start()
    {
        Observable.EveryEndOfFrame().Subscribe((x) => rectReactive.Value = _parentImage.fillAmount).AddTo(this);
        rectReactive.Subscribe((x) => _rect.localScale = new Vector3(1, x, 1)).AddTo(this);
    }
}
