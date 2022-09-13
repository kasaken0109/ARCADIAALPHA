using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEventController : MonoBehaviour
{
    [SerializeField]
    GameObject[] _footEffects;
    [SerializeField]
    GameObject _footEffectPrefab;
    Rigidbody _rb;
    // Start is called before the first frame update
    void Start()
    {
        TryGetComponent(out _rb);
    }

    public void ActiveFootEffect(int value)
    {
        Instantiate(_footEffectPrefab, _footEffects[value].transform.position, GameManager.Player.transform.rotation);
    }
}
