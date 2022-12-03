using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class SizeRescaler : MonoBehaviour
{
    [SerializeField]
    float _originSize = 0.8f;
    [SerializeField]
    float _expandSize = 1;
    RectTransform rectTransform;
    // Start is called before the first frame update
    void Start()
    {
        TryGetComponent(out rectTransform);
    }

    public void ScaleShrink()
    {
        rectTransform.DOScale(new Vector3(_originSize, _originSize, _originSize), 1f);
    }

    public void ScaleExpand()
    {
        rectTransform.DOScale(new Vector3(_expandSize, _expandSize, _expandSize), 1f);
    }
}
