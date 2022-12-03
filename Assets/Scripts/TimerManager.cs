
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// タイマーの表示と制御を行う
/// </summary>
public class TimerManager : MonoBehaviour
{
    /// <summary>現在の時間</summary>
    int time;
    /// <summary>ゲームプレイ中か</summary>
    bool isPlaying = false;
    /// <summary>ゲームプレイ中か</summary>
    public bool IsPlaying { set => isPlaying = value; }
    // Start is called before the first frame update

    /// <summary>
    /// タイマーのカウントダウンを進める
    /// </summary>
    /// <returns></returns>
    public IEnumerator TimeUpdateCountUp()
    {
        time = 0;
        while (true)
        {
            if (isPlaying)
            {
                time++;
                yield return new WaitForSeconds(1);
            }

            yield return null;
        }
    }

    /// <summary>
    /// クエストの結果を保存
    /// </summary>
    public void SaveTime()
    {
        if (GameManager.Instance.GameStatus == GameState.PLAYERWIN)
        {
            //経過時間を計算
            PlayerPrefs.SetInt("TimeScore", time);
            PlayerPrefs.Save();
            isPlaying = false;
        }
        else if (GameManager.Instance.GameStatus == GameState.PLAYERLOSE)
        {
            PlayerPrefs.SetInt("TimeScore", -1);
            PlayerPrefs.Save();
        }
    }
}
