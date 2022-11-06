using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

/// <summary>
/// 弾の選択処理を行う
/// </summary>
public class BulletSelectController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("射撃を管理するクラス")]
    private BulletFire _bulletFire = default;

    [SerializeField]
    [Tooltip("弾を設定するリスト")]
    private List<Bullet> _IDs;

    [SerializeField]
    [Tooltip("初期化時の弾のリスト")]
    private BulletList _init;

    [SerializeField]
    private EquipBulletDisplay[] _bulletDisplay;

    private int equipID = 0;

    public List<Bullet> MyBullet { set { _IDs = value; } }

    void Start()
    {
        EquipmentInit();
        _bulletDisplay.ToList().ForEach(c => c.SetBulletInformation());
        EquipBullets();
        SelectBullet(equipID);
    }

    /// <summary>
    ///　装備の状態を初期化する
    /// </summary>
    private void EquipmentInit()
    {
        var equip = EquipmentManager.Instance.Equipments;
        for (int i = 0; i < equip.Length; i++) equip[i] = equip[i] == null ? _init.Bullets[i] : equip[i];
    }

    /// <summary>
    /// 弾を選択する
    /// </summary>
    /// <param name="bullet">装備する弾のインデックス</param>
    public void SelectBullet(int bullet)
    {
        FindObjectOfType<BulletFire>().CurrentEquipID = bullet;
        _bulletFire.EquipBullet(_IDs[bullet]);
        for (int i = 0; i < _bulletDisplay.Length; i++)
        {
            if (i == bullet) _bulletDisplay[i].Active();
            else _bulletDisplay[i].NonActive();
        }
    }

    /// <summary>
    /// EquipmentManegerに設定されている装備を選択
    /// </summary>
    private void EquipBullets()
    {
        var equipM = EquipmentManager.Instance;
        for (int i = 0;i < _IDs.Count;i++) _IDs[i] = equipM.Equipments[i] ? equipM.Equipments[i] : _init.Bullets[i];
    }

    private void OnDestroy()
    {
        equipID = 0;
    }
}
