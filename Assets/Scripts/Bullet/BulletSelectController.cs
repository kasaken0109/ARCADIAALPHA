using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

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
    [Tooltip("弾の選択UI")]
    private GameObject[] UI;

    [SerializeField]
    private Animator _anim;

    private BulletSelectDisplay _bulletDisplay;

    private int equipID = 0;

    private bool IsPush = false;


    public List<Bullet> MyBullet { set { _IDs = value; } }



    // Start is called before the first frame update
    void Start()
    {
        TryGetComponent(out _bulletDisplay);
        EquipmentInit();

        _bulletDisplay.BulletInformationInit(EquipmentManager.Instance.Equipments);
        EquipBullets();
        SelectBullet(equipID);//弾の選択状態の初期化
        _bulletDisplay.MoveSelectFrame(equipID);
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
    /// 弾の選択UIの表示状態を切り替える
    /// </summary>
    public void OpenBulletMenu()
    {
        IsPush = !IsPush;
        _anim.SetBool("IsPush", IsPush);
        Vector3 scale = IsPush ? Vector3.zero : Vector3.one;
        foreach (var item in UI) item.transform.localScale = scale;
    }

    /// <summary>
    /// 入力値に応じて装備関数を実行する
    /// </summary>
    /// <param name="scrollValue"></param>
    public void SelectBullet(float scrollValue)
    {
        if (!IsPush) return;
        equipID = scrollValue > 0 ? (equipID == 0 ? 2 : equipID - 1) : (equipID == 2 ? 0 : equipID + 1);
        SelectBullet(equipID);
        _bulletDisplay.MoveSelectFrame(equipID);
        if(!_IDs[equipID]) SelectBullet(scrollValue);
    }

    /// <summary>
    /// 弾を選択する
    /// </summary>
    /// <param name="bullet">装備する弾のインデックス</param>
    public void SelectBullet(int bullet)
    {
        _bulletFire.EquipBullet(_IDs[bullet]);
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
