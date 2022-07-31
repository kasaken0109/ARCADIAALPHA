using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AttackInformation")]
public class AttackMotion : ScriptableObject
{
    [SerializeField]
    string _attackClipName = default;

    [SerializeField]
    AttackInfomation[] _attackInfomations;

    [SerializeField]
    float _clipDuraration;

    [SerializeField]
    bool _canMove = false;

    public string AttackClipName => _attackClipName;

    public AttackInfomation[] AttackInformations => _attackInfomations;

    public float ClipDuraration => _clipDuraration;

    public bool CanMove => _canMove;


}
