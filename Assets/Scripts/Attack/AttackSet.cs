using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���ɍĐ�����X�e�[�g���ƑJ�ڂ��锻�莮�̏�������
/// </summary>
[System.Serializable]
public class AttackSet
{
    [Tooltip("���̃X�e�[�g��")]
    public string _NextStateName = "";

    [SerializeReference,SubclassSelector]
    [Tooltip("����N���X")]
    public ICondition _condition = default;
}
