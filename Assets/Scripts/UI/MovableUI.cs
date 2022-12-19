using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum MoveType
{
    World,
    Local,
}
public class MovableUI : MonoBehaviour
{
    [SerializeField]
    protected MoveType _moveType = MoveType.Local;
    [SerializeField]
    protected float _moveDuraration = 0.5f;
    [SerializeField]
    protected Vector3 _targetPos = new Vector3(-1498.2f,0,0);
    [SerializeField]
    protected Vector3 _deltaPos = new Vector3(0, 451.82f, 0);
    [SerializeField]
    protected Ease _animType = Ease.Linear;

    RectTransform _rectTransform = default;
    protected Vector3 _originPos;
    public bool IsCompleted { get => isCompleted; }

    protected bool isCompleted = false;

    Vector3 _delta = Vector3.zero;

    private void Awake()
    {
        TryGetComponent(out _rectTransform);
        SetUpOriginPos();
    }

    public void SetDelta(bool isSet)
    {
        _delta = isSet ? _deltaPos : Vector3.zero;
    }

    protected void SetUpOriginPos()
    {
        switch (_moveType)
        {
            case MoveType.World:
                _originPos = _rectTransform.transform.position;
                break;
            case MoveType.Local:
                _originPos = _rectTransform.transform.localPosition;
                break;
            default:
                break;
        }
    } 
    public virtual void Move(bool isDisplay)
    {
        var targetPos = isDisplay ? _targetPos + _originPos +_delta: _originPos + _delta;
        switch (_moveType)
        {
            case MoveType.World:
                _rectTransform.DOMove(targetPos, _moveDuraration).SetEase(_animType).OnStart(() => isCompleted = false).OnComplete(() => isCompleted = true);
                break;
            case MoveType.Local:
                _rectTransform.DOLocalMove(targetPos, _moveDuraration).SetEase(_animType).OnStart(() => isCompleted = false).OnComplete(() => isCompleted = true);
                break;
            default:
                break;
        }
    }

    public void Reset()
    {
        isCompleted = false;
    }
}
