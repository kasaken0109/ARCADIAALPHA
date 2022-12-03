using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class EquipButtonSelect : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField]
    GameObject _selectTargt = default;
    [SerializeField]
    GameObject _display = default;

    Outline outline;
    // Start is called before the first frame update
    void Awake()
    {
        _selectTargt.TryGetComponent(out outline);
    }
    public void OnSelect(BaseEventData eventData)
    {
        outline.enabled = true;
        _display.SetActive(true);
    }
    public void OnDeselect(BaseEventData eventData)
    {
        outline.enabled = false;
        _display.SetActive(false);
    }

    
}
