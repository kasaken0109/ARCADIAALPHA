using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttributeController : MonoBehaviour
{
    [SerializeField]
    float _mpAbsorbValue = 0.2f;

    [SerializeField]
    float _attackPower = 80f;

    [SerializeField]
    int _stunPower = 0;
    [SerializeField]
    int _healPower = 0;

    public float MPAbsorbValue => _mpAbsorbValue;
    public float AttackPower => _attackPower;
    public int StunPower => _stunPower;
    public int HealPower => _healPower;
}
