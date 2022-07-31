using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class InputActivater : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        TouchSimulation.Enable();
    }
}
