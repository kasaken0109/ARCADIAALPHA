using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// 弾の情報を表示させるUI
/// </summary>
[System.Serializable]
public struct BulletInformation
{
    [Tooltip("弾を表示するUI")]
    public Image _NameDisplay;

    [Tooltip("弾を表示するバックグラウンドUI")]
    public Image _NameBackgroundDisplay;

    [Tooltip("スキルを表示するUI")]
    public Image _skillDisplay;
}

/// <summary>
/// 弾の選択時のUIの挙動を制御する
/// </summary>
public class BulletSelectDisplay : MonoBehaviour
{
    [SerializeField]
    [Tooltip("弾の選択画面の弾情報表示UI")]
    BulletInformation[] _bulletInformations = default;

    /// <summary>
    /// 弾の情報を初期化する
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
