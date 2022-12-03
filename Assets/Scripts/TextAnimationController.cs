using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TextAnimationController : MonoBehaviour
{
    [SerializeField]
    string _animationMessage = "";
    [SerializeField]
    Text _text = default;
    [SerializeField]
    Ease _animationType = Ease.Linear;

    void Start()
    {
        _text.DOText(_animationMessage, 2).SetEase(_animationType);
    }
}
