using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class OnSelectEventController : MonoBehaviour, ISelectHandler
{
    [SerializeField]
    UnityEvent _onSelect = default;

    public void OnSelect(BaseEventData eventData)
    {
        _onSelect?.Invoke();
    }
}
