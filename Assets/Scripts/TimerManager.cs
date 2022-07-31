
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// タイマーの表示と制御を行う
/// </summary>
public class TimerManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("クエストの制限時間")]
    private int _timeLimit = 180;

    [SerializeField]
    [Tooltip("時間切れの警告時間")]
    private int _timeWarning = 20;

    [SerializeField]
    [Tooltip("残り時間の表示テキスト")]
    private Text m_timeText = null;

    [SerializeField]
    private Animation m_anim = null;

    /// <summary>現在の時間</summary>
    int time;
    /// <summary>ゲームプレイ中か</summary>
    bool isPlaying = false;
    /// <summary>ゲームプレイ中か</summary>
    public bool IsPlaying { set => isPlaying = value; }
    // Start is called before the first frame update
    void Awake()
    {
        TimerSetUp();
    }

    /// <summary>
    /// タイマーの初期設定を行う
    /// </summary>
    private void TimerSetUp()
    {
        //クリアランク判定用にクエスト制限時間を保存
        PlayerPrefs.SetInt("MaxTime", _timeLimit);
        PlayerPrefs.Save();
        ///
        time = _timeLimit;
    }

    /// <summary>
    /// タイマーのカウントダウンを進める
    /// </summary>
    /// <returns></returns>
    public IEnumerator TimeUpdate()
    {
        m_timeText.text = "制限時間：" + time;
        while (time > 0)
        {
            if (isPlaying)
            {
                time--;
                m_timeText.text = "制限時間：" + time;
                yield return new WaitForSeconds(1);
                if (time <= _timeWarning)
                {
                    //アニメーションとフォントの色を変え、警告
                    m_timeText.color = Color.red;
                    m_anim.Play();
                }
            }
            
            yield return null;
        }
        //時間切れになったらプレイヤーの負け
        GameManager.Instance.SetGameState(GameState.PLAYERLOSE);
    }

    /// <summary>
    /// クエストの結果を保存
    /// </summary>
    public void SaveTime()
    {
        if (GameManager.Instance.GameStatus == GameState.PLAYERWIN)
        {
            //経過時間を計算
            PlayerPrefs.SetInt("TimeScore", _timeLimit - time);
            PlayerPrefs.Save();
            isPlaying = false;
        }
        else if (GameManager.Instance.GameStatus == GameState.PLAYERLOSE)
        {
            PlayerPrefs.SetInt("TimeScore", _timeLimit);
            PlayerPrefs.Save();
        }
    }
}
