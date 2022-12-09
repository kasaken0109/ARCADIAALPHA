using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �����̃f�[�^�A�\�����q���N���X
/// </summary>
public class EquipDataPresenter : MonoBehaviour
{
    [SerializeField]
    [Tooltip("�f�[�^�ϊ��p��Bullet�f�[�^")]
    Bullet[] _bullets = default;

    [SerializeField]
    [Tooltip("�e�̃��b�N��")]
    bool[] _lockStates = default;

    [SerializeField]
    [Tooltip("�f�[�^�ϊ��p��Bullet�f�[�^")]
    CustomSkill[] _skills = default;

    [SerializeField]
    BulletList _initList = default;

    [SerializeField]
    EquipmentView _equipmentView = default;

    private void Awake()
    {
        ServiceLocator.SetInstance<EquipDataPresenter>(this);
    }

    public int GetBulletLength() => _bullets.Length;

    public int GetSkillLength() => _skills.Length;

    /// <summary>
    /// �e�̃f�[�^��EquipmentManager�ɑ��M����
    /// </summary>
    /// <param name="id">�Ώۂ�id</param>
    /// <returns>�Ώۂ̒e�f�[�^</returns>
    public void SendBulletData(int id)
    {
        if (id < 0 || id >= _bullets.Length)
        {
            Debug.LogError($"_bullets{id} is null : �Ώۂ�ID�̒e�f�[�^������܂���B");
            return;

        }
        EquipmentManager.Instance.SetEquipments(_bullets[id]);
    }

    /// <summary>
    /// �e�̃f�[�^��EquipmentManager�ɑ��M����
    /// </summary>
    /// <param name="id">�Ώۂ�id</param>
    /// <returns>�Ώۂ̒e�f�[�^</returns>
    public void SendSkillData(int id)
    {
        if (id < 0 || id >= _skills.Length)
        {
            Debug.LogError($"_skills{id} is null : �Ώۂ�ID�̃X�L���f�[�^������܂���B");
            return;

        }
        var equip = EquipmentManager.Instance;
        equip.Equipments[equip.GetEquipID].PassiveSkill = _skills[id];
    }

    /// <summary>
    /// �e�̃f�[�^��EquipmentManager�ɑ��M����
    /// </summary>
    /// <param name="id">�Ώۂ�id</param>
    /// <returns>�Ώۂ̒e�f�[�^</returns>
    public CustomSkill GetSkillData(int id)
    {
        if (id < 0 || id >= _skills.Length)
        {
            Debug.LogError($"_skill{id} is null : �Ώۂ�ID�̃X�L���f�[�^������܂���B");
            return null;

        }
        return _skills[id];
    }


    public Bullet GetBulletData(int id)
    {
        if (id < 0 || id >= _bullets.Length)
        {
            Debug.LogError($"_bullets{id} is null : �Ώۂ�ID�̒e�f�[�^������܂���B");
            return null;

        }
        return _bullets[id];
    }

    public Bullet GetInitBullet(int id)
    {
        return _initList.Bullets[id];
    }

    private void OnDestroy()
    {
        ServiceLocator.RemoveInstance<EquipDataPresenter>();
    }

}
