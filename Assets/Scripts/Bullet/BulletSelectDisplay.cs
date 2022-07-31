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
    [Tooltip("名前を表示するUI")]
    public Text _NameDisplay;

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

    [SerializeField]
    [Tooltip("選択フレームの表示位置")]
    RectTransform _framePosition = default;

    [SerializeField]
    [Tooltip("フレームが動く対象の位置")]
    RectTransform[] _movePosition = default;

    [SerializeField]
    [Tooltip("フレームが移動にかかる時間")]
    float _moveDuraration = 0.5f;

    /// <summary>
    /// 弾の情報を初期化する
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
    /// フレームを指定のポイントに移動させる
    /// </summary>
    /// <param name="index">移動ポイントのインデックス</param>
    public void MoveSelectFrame(int index)
    {
        _framePosition.DOMove(_movePosition[index].position, _moveDuraration);
    }
}
