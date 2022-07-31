using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �e�̏����{�^���ɓo�^����
/// </summary>
public class BulletSelector : MonoBehaviour
{
    [SerializeField]
    [Tooltip("�o���b�g���X�g")]
    BulletList _list = default;
    [SerializeField]
    [Tooltip("�I��Ώۂ̃{�^��")]
    ButtonSelector[] _buttons = default;

    void Awake()
    {
        BulletInformationInit();
    }

    /// <summary>
    /// �e�̏�������������
    /// </summary>
    public void BulletInformationInit()
    {
        for (int i = 0; i < _list.Bullets.Count; i++)
        {
            _buttons[i].SetInformation(_list.Bullets[i]);
        }
    }
}
