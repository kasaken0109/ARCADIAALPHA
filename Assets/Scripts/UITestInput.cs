using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class UITestInput : MonoBehaviour
{
    PlayerInput inputAction;
    EventSystem eventSystem;
    GameObject current;

    private void Awake()
    {
        TryGetComponent(out inputAction);
        eventSystem = FindObjectOfType<EventSystem>();
    }
    private void OnEnable()
    {
        //inputAction.actions["Move"].started += OnSelect;
    }
    
    public void OnSelect(InputAction.CallbackContext context)
    {

    }

    public void AddNum(int value)
    {
        var m = eventSystem.currentSelectedGameObject.GetComponentInParent<Text>();
        m.text = (int.Parse(m.text) + value).ToString();
    }

    public void SelectObj(GameObject gameObject)
    {
        //eventSystem.SetSelectedGameObject(gameObject);
    }
}
