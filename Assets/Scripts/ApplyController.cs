using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アプリの挙動を制御する(仮)
/// </summary>
public class ApplyController : MonoBehaviour
{
    [SerializeField]
    string titleSceneName = "TitleScene";
    [SerializeField]
    Bullet[] bullets = default;
    /// <summary>
    /// アプリを終了する
    /// </summary>
    public void Quit()
    {
        Application.Quit();
    }

    public void OnBackTitleScene()
    {
        InitApplication();
        SceneLoader.Instance.SceneLoad(titleSceneName);
    }

    public void InitApplication()
    {
        PlayerPrefs.SetInt("IsFirst", 0);
        PlayerPrefs.Save();
        for (int i = 0; i < bullets.Length; i++)
        {
            bullets[i].IsUnlock = false;
            bullets[i].PassiveSkill = null;
        }
        var manager = EquipmentManager.Instance;
        manager.Point.Value = 3;
        for (int i = 0; i < 3; i++)
        {
            manager.SetEquipID(i);
            manager.SetEquipments(null);
        }
        manager.SetEquipID(0);
        //ServiceLocator.GetInstance<EquipmentView>().InitIcons();
    }

    private void Start()
    {
        if(PlayerPrefs.GetInt("IsFirst") == 0)
//#if UNITY_EDITOR
        InitApplication();
//#endif
    }

}
