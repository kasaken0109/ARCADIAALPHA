using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class EventSystemselector : MonoBehaviour
{
    [SerializeField]
    GameObject[] _selecteds;

    EventSystem eventSystem;

    // Start is called before the first frame update
    void Start()
    {
        TryGetComponent(out eventSystem);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(eventSystem.currentSelectedGameObject);
    }
}
