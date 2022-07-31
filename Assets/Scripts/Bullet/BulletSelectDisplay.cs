using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// �e�̏���\��������UI
/// </summary>
[System.Serializable]
public struct BulletInformation
{
    [Tooltip("���O��\������UI")]
    public Text _NameDisplay;

    [Tooltip("�X�L����\������UI")]
    public Image _skillDisplay;
}

/// <summary>
/// �e�̑I������UI�̋����𐧌䂷��
/// </summary>
public class BulletSelectDisplay : MonoBehaviour
{
    [SerializeField]
    [Tooltip("�e�̑I����ʂ̒e���\��UI")]
    BulletInformation[] _bulletInformations = default;

    [SerializeField]
    [Tooltip("�I���t���[���̕\���ʒu")]
    RectTransform _framePosition = default;

    [SerializeField]
    [Tooltip("�t���[���������Ώۂ̈ʒu")]
    RectTransform[] _movePosition = default;

    [SerializeField]
    [Tooltip("�t���[�����ړ��ɂ����鎞��")]
    float _moveDuraration = 0.5f;

    /// <summary>
    /// �e�̏�������������
    /// </summary>
    /// <param name="bullets"></param>
    public void BulletInformationInit(Bullet[] bullets)
    {
        for (int i = 0; i < bullets.Length; i++)
        {
            _bulletInformations[i]._NameDisplay.text = bullets[i].Name;
            _bulletInformations[i]._skillDisplay.sprite = bullets[i].passiveSkill?.ImageBullet;
        }
    }

    /// <summary>
    /// �t���[�����w��̃|�C���g�Ɉړ�������
    /// </summary>
    /// <param name="index">�ړ��|�C���g�̃C���f�b�N�X</param>
    public void MoveSelectFrame(int index)
    {
        _framePosition.DOMove(_movePosition[index].position, _moveDuraration);
    }
}
