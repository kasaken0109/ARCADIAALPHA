using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SelectFrameController : MonoBehaviour
{
    [SerializeField]
    RectTransform[] _movePoints = default;
    [SerializeField]
    GameObject _frame = default;
    [SerializeField]
    float _duraration = 0.5f;
    RectTransform moveRect = default;

    bool IsComoleted;
    // Start is called before the first frame update
    void Awake()
    {
        ServiceLocator.SetInstance<SelectFrameController>(this);
        _frame.TryGetComponent(out moveRect);
    }

    private void OnDestroy()
    {
        ServiceLocator.RemoveInstance<SelectFrameController>();
    }

    public void MoveFrame(int posId)
    {
        if(posId < 0 || posId >= _movePoints.Length)
        {
            Debug.LogError("outBounds_!");
            return;
        }
        moveRect.DOLocalMove(_movePoints[posId].localPosition, _duraration).SetEase(Ease.Linear).WaitForStart();
    }
}
