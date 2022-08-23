using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PanelAnimationController : MonoBehaviour
{
    [SerializeField]
    Transform _targetPos;

    Vector3 originScale;
    Vector3 originPosition;
    // Start is called before the first frame update
    void Start()
    {
        originScale = transform.localScale;
        originPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FadePanel()
    {
        transform.DOMove(_targetPos.position, 1).OnComplete(() => gameObject.SetActive(false));
    }

    private void OnEnable()
    {
        transform.DOScale(1, 1);
    }

    private void OnDisable()
    {
        transform.localScale = originScale;
        transform.position = originPosition;
    }
}
