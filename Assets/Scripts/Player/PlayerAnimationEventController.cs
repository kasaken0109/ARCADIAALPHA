using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEventController : MonoBehaviour
{
    [SerializeField]
    GameObject[] _footEffects;
    Rigidbody _rb;
    // Start is called before the first frame update
    void Start()
    {
        TryGetComponent(out _rb);
    }

    public void ActiveFootEffect(int value)
    {
        _footEffects[value].SetActive(true);
    }
}
