using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Audio;

/// <summary>
/// シーンでSoundManagerを呼び出す用のクラス
/// </summary>
public class SoundCaller : MonoBehaviour
{
    [SerializeField]
    [Tooltip("シーンに設定するBGM")]
    AudioClip _bgm;
    [SerializeField]
    [Tooltip("ボタンに登録するSEがDefautかどうか")]
    bool _canSetSE = true;

    void Start()
    {
        //BGM再生
        if(_bgm)SoundManager.Instance.PlayBGM(_bgm);
        if (_canSetSE)
        {
            //ボタンを検索、音の再生処理を追加する
            var button = FindObjectsOfType<Button>();
            button.ToList().ForEach(x => x.onClick.AddListener(() => SoundManager.Instance.PlayClick()));
        }
    }

    /// <summary>
    /// OnClick用のクリック音再生処理
    /// </summary>
    /// <param name="clip">クリック音</param>
    public void PlayButtonSE(AudioClip clip)
    {
        SoundManager.Instance.PlayClick(clip);
    }
}
