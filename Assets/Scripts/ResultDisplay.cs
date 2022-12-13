using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// リザルト画面の処理を行う
/// </summary>
public class ResultDisplay : MonoBehaviour
{
    [SerializeField]
    [Tooltip("タイム表示用のテキスト")]
    TextMeshProUGUI m_time = null;

    [SerializeField]
    [Tooltip("ランク表示用のテキスト")]
    TextMeshProUGUI m_rank = null;

    [SerializeField]
    [Tooltip("シーン遷移用のボタン")]
    GameObject[] m_buttons;

    [SerializeField]
    [Tooltip("シーン遷移用のボタン表示まで待つ時間")]
    float m_waitDisplay = 1f;

    /// <summary>クリアまでにかかった時間</summary>
    int clearTime;
    /// <summary>制限時間</summary>
    int maxTime;
    /// <summary>ランクの文字列</summary>
    string rank;
    /// <summary>ランクを判別する数値</summary>
    int clearRank;
    /// <summary>delta値</summary>
    const float delta = 0.01f;
    /// <summary>ランク計算用係数</summary>
    const int calcValue = 10;
    /// <summary>deltaTimeのWaitForSeconds</summary>
    WaitForSeconds deltaTime = new WaitForSeconds(delta);
    /// <summary>表示遅延用のWaitForSeconds</summary>
    WaitForSeconds delayDisplay = new WaitForSeconds(0.5f);

    void Start()
    {
        SetRank();
        StartCoroutine(nameof(DisplayResult));
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
        clearRank = clearTime < 0 ? 6
                    : calc <= 1 ? 0
                    : calc <= 2 ? 1
                    : calc <= 4 ? 2
                    : calc <= 8 ? 3
                    : calc <= 16 ? 4
                    : 5;//(clearTime * calcValue) / maxTime;
        EquipmentManager.Instance.Point += clearRank < 2 ? 2 : clearRank != 6 ? 1 : 0;
        switch (clearRank)
        {
            case 0:
                rank = "SS";
                break;
            case 1:
                rank = "S";
                break;
            case 2:
                rank = "A";
                break;
            case 3:
                rank = "B";
                break;
            case 4:
                rank = "C";
                break;
            case 5:
                rank = "D";
                break;
            case 6:
                rank = "Failed...";
                break;
        }
    }

    /// <summary>
    /// クエスト結果を表示する
    /// </summary>
    /// <returns></returns>
    IEnumerator DisplayResult()
    {
        float time = 0;
        //スロット演出
        while (time < m_waitDisplay)
        {
            m_time.text = Random.Range(1, 1000).ToString();
            time += delta;
            yield return deltaTime;
        }
        //ランク表示
        m_time.text = clearTime < 0 ? "----" : clearTime.ToString();
        yield return delayDisplay;
        m_rank.text = rank;
        //シーン遷移用ボタン表示
        foreach (var item in m_buttons)
        {
            yield return delayDisplay;
            item.SetActive(true);
        }
    }
}
