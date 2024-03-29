using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 装備のデータ、表示を繋ぐクラス
/// </summary>
public class EquipDataPresenter : MonoBehaviour
{
    [SerializeField]
    [Tooltip("データ変換用のBulletデータ")]
    Bullet[] _bullets = default;

    [SerializeField]
    [Tooltip("弾のロック状況")]
    UnlockDisplayController[] _lockStates = default;

    [SerializeField]
    [Tooltip("データ変換用のBulletデータ")]
    CustomSkill[] _skills = default;

    [SerializeField]
    BulletList _initList = default;

    [SerializeField]
    EquipmentView _equipmentView = default;

    private void Awake()
    {
        ServiceLocator.SetInstance<EquipDataPresenter>(this);
    }

    public void InitUnlock()
    {
        for (int i = 0; i < _bullets.Length; i++)
        {
            _lockStates[i].UnlockDisplay(_bullets[i].IsUnlock);
        }
    }

    public int GetBulletLength() => _bullets.Length;

    public int GetSkillLength() => _skills.Length;

    public void SetUnlock(int unlockID)
    {
        _lockStates[unlockID].UnlockDisplay(true);
    }

    /// <summary>
    /// 弾のデータをEquipmentManagerに送信する
    /// </summary>
    /// <param name="id">対象のid</param>
    /// <returns>対象の弾データ</returns>
    public void SendBulletData(int id)
    {
        if (id < 0 || id >= _bullets.Length)
        {
            Debug.LogError($"_bullets{id} is null : 対象のIDの弾データがありません。");
            return;

        }
        var view = ServiceLocator.GetInstance<EquipmentView>();
        var manager = EquipmentManager.Instance;
        view.SetEquipIcon(manager.Equipments[manager.GetEquipID].BulletID, 0);
        manager.SetEquipments(_bullets[id]);
        view.SetEquipIcon(id, manager.GetEquipID + 1);
    }

    /// <summary>
    /// 弾のデータをEquipmentManagerに送信する
    /// </summary>
    /// <param name="id">対象のid</param>
    /// <returns>対象の弾データ</returns>
    public void SendSkillData(int id)
    {
        if (id < 0 || id >= _skills.Length)
        {
            Debug.LogError($"_skills{id} is null : 対象のIDのスキルデータがありません。");
            return;

        }
        var equip = EquipmentManager.Instance;
        equip.Equipments[equip.GetEquipID].PassiveSkill = _skills[id];
    }

    /// <summary>
    /// 弾のデータをEquipmentManagerに送信する
    /// </summary>
    /// <param name="id">対象のid</param>
    /// <returns>対象の弾データ</returns>
    public CustomSkill GetSkillData(int id)
    {
        if (id < 0 || id >= _skills.Length)
        {
            Debug.LogError($"_skill{id} is null : 対象のIDのスキルデータがありません。");
            return null;

        }
        return _skills[id];
    }


    public Bullet GetBulletData(int id)
    {
        if (id < 0 || id >= _bullets.Length)
        {
            Debug.LogError($"_bullets{id} is null : 対象のIDの弾データがありません。");
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
