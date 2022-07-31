using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// シーン遷移を行うクラス
/// </summary>
public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; } 
    [SerializeField]
    [Tooltip("移動するシーン")]
    string m_LoadSceneName = "ResultScene";

    [SerializeField]
    [Tooltip("シーン遷移開始するまでの時間")]
    float m_waitTime = 0;

    [SerializeField]
    [Tooltip("Fade画面の演出スピード")]
    float m_fadeSpeed = 1f;

    [SerializeField]
    [Tooltip("FadePanel")]
    Image m_loadPanel = null;

    /// <summary>ロード中か</summary>
    bool m_isLoading = false;

    const float maxValue = 0.99f;
    const float fillSpeed = 0.02f;
    const float fadeFixSpeed = 0.1f;

   void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// シーン遷移を行う
    /// </summary>
    /// <returns></returns>
    IEnumerator Load()
    {
        if (m_isLoading)
        {
            yield return new WaitForSeconds(m_waitTime);
            //タイプごとに処理を分ける
            if(m_loadPanel.type == Image.Type.Filled)
            {
                while (m_loadPanel.fillAmount < maxValue)
                {
                    m_loadPanel.fillAmount += fillSpeed;
                    yield return new WaitForSeconds(fadeFixSpeed / m_fadeSpeed);
                }
            }
            else
            {
                var color = m_loadPanel.color;
                while (m_loadPanel.color.a < maxValue)
                {
                    color.a += fillSpeed;
                    m_loadPanel.color = color;
                    yield return new WaitForSeconds(fadeFixSpeed / m_fadeSpeed);
                }
            }
            
            SceneManager.LoadScene(m_LoadSceneName);
        }
    }

    /// <summary>
    /// シーン遷移を開始する(デフォルト)
    /// </summary>
    public void SceneLoad()
    {
        m_isLoading = true;
        StartCoroutine(nameof(Load));
    }

    /// <summary>
    /// 対象のシーン遷移を開始する
    /// </summary>
    /// <param name="sceneName">対象のシーン名</param>
    public void SceneLoad(string sceneName)
    {
        m_isLoading = true;
        m_LoadSceneName = sceneName;
        StartCoroutine(nameof(Load));
    }

    /// <summary>
    /// 前のシーンをロードする
    /// </summary>
    public void SceneReload()
    {
        m_isLoading = true;
        m_LoadSceneName = PlayerPrefs.GetString("SceneName");
        StartCoroutine(nameof(Load));
    }
}
