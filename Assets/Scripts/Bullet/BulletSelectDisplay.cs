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
    [Tooltip("�e��\������UI")]
    public Image _NameDisplay;

    [Tooltip("�e��\������o�b�N�O���E���hUI")]
    public Image _NameBackgroundDisplay;

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

    /// <summary>
    /// �e�̏�������������
    /// </summary>
    /// <param name="bullets"></param>
    public void BulletInformationInit(Bullet[] bullets)
    {
        for (int i = 0; i < bullets.Length; i++)
        {
            _bulletInformations[i]._NameDisplay.sprite = bullets[i].EquipImage;
            _bulletInformations[i]._skillDisplay.sprite = bullets[i].passiveSkill?.ImageBullet;
        }
    }
}
