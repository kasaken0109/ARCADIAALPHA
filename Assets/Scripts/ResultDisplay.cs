using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// リザルト画面の処理を行う
/// </summary>
public class ResultDisplay : MonoBehaviour
{
    [SerializeField]
    [Tooltip("タイム表示用のテキスト")]
    Text _timeDisplay = default;

    [SerializeField]
    [Tooltip("ランクのテキストオブジェクト")]
    GameObject _rankLabel = default;

    [SerializeField]
    [Tooltip("アニメーションさせる画像オブジェクト")]
    RectTransform[] _moveObjs = default;

    [SerializeField]
    [Tooltip("フェードアニメーションさせる画像アイコン")]
    Image _fadeIcon = default;

    [SerializeField]
    [Tooltip("ランク表示用の画像ホルダー")]
    Image _rank = default;

    [SerializeField]
    [Tooltip("獲得ポイント表示用のテキスト")]
    Image _point = default;

    [SerializeField]
    [Tooltip("ランクの画像素材")]
    Sprite[] _rankImageSources = default;

    [SerializeField]
    [Tooltip("シーン遷移用のボタン")]
    GameObject[] _buttons;

    [SerializeField]
    [Tooltip("スライドインのアニメーション時間")]
    float _slideInDuraration = 1f;

    [SerializeField]
    [Tooltip("シーン遷移用のボタン表示まで待つ時間")]
    float _waitDisplay = 1f;

    /// <summary>クリアまでにかかった時間</summary>
    int clearTime;
    /// <summary>獲得ポイント</summary>
    int getPoint;
    /// <summary>ランクを判別する数値</summary>
    int clearRank;
    /// <summary>表示遅延用のWaitForSeconds</summary>
    WaitForSeconds delayDisplay = new WaitForSeconds(0.5f);

    void Start()
    {
        SetRank();
    }

    /// <summary>
    /// ランクを計算する
    /// </summary>
    void SetRank()
    {
        //保存されている値を取得
        clearTime = PlayerPrefs.GetInt("TimeScore");
        //maxTime = PlayerPrefs.GetInt("MaxTime");
        var calc = Mathf.CeilToInt((float)clearTime / 15f);
        //ランク計算
        clearRank = clearTime < 0 ? 4
                    : calc <= 1 ? 0
                    : calc <= 2 ? 1
                    : calc <= 4 ? 2
                    : 3;//(clearTime * calcValue) / maxTime;
        getPoint = clearRank < 2 ? 2 : clearRank != 4 ? 1 : 0;
        EquipmentManager.Instance.Point += getPoint;
        _rank.sprite = _rankImageSources[clearRank];
        _rank.enabled = false;
    }

    string ConvertDisplayTimeText(int time)
    {
        int seconds = time % 60;
        int minutes = time / 60;
        int hours = time / 3600;
        return $"{hours}:{minutes}:{seconds}";
    }



    /// <summary>
    /// クエスト結果を表示する
    /// </summary>
    /// <returns></returns>
    public void DisplayResult()
    {
        Sequence anim = DOTween.Sequence()
            .Append(_moveObjs[0].DOLocalMoveY(0, _slideInDuraration))
            .Join(_moveObjs[1].DOLocalMoveY(0, _slideInDuraration))
            .Join(_fadeIcon.DOFade(1f, _slideInDuraration))
            .AppendInterval(_waitDisplay)
            .Append(_moveObjs[2].DOLocalMoveX(0, _slideInDuraration))
            .AppendInterval(_waitDisplay)
            .AppendCallback(() => _timeDisplay.text = ConvertDisplayTimeText(clearTime))
            .AppendInterval(_waitDisplay)
            .Append(_moveObjs[3].DOLocalMoveX(0, _slideInDuraration))
            .AppendCallback(() => _timeDisplay.text = ConvertDisplayTimeText(clearTime))
            .AppendInterval(_waitDisplay)
            .AppendCallback(() => _rankLabel.SetActive(true))
            .AppendInterval(_waitDisplay)
            .AppendCallback(() => _rank.enabled = true)
            .AppendInterval(_waitDisplay)
            .AppendCallback(() =>
            {
                _buttons[0].SetActive(true);
                _buttons[1].SetActive(true);
            });
    }
}
