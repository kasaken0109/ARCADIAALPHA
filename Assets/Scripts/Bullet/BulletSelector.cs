using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 弾の情報をボタンに登録する
/// </summary>
public class BulletSelector : MonoBehaviour
{
    [SerializeField]
    [Tooltip("バレットリスト")]
    BulletList _list = default;
    [SerializeField]
    [Tooltip("選択対象のボタン")]
    ButtonSelector[] _buttons = default;

    void Awake()
    {
        BulletInformationInit();
    }

    /// <summary>
    /// 弾の情報を初期化する
    /// </summary>
    public void BulletInformationInit()
    {
        for (int i = 0; i < _list.Bullets.Count; i++)
        {
            _buttons[i].SetInformation(_list.Bullets[i]);
        }
    }
}
