using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.IO;


public interface IPassiveSkill
{ 
    bool IsAvailable { get; set; }
    void UsePassiveSkill();
}

public interface IActionSkill
{
    bool IsAvailable { get; set; }
    void UseActionSkill();
}

public enum SkillType
{
    AttackBuff,
    DefenceBuff,
    Speed,
    EffectBuff,
}

[Serializable]
public struct SaveData
{
    private List<bool> m_usePassiveList;

    public List<bool> UsePassiveList { set { m_usePassiveList = value; } }
}

public static class DataManager
{
    public static SaveData data;

    const string DATA_FILE_PATH = "save.json";

    public static void SavePassiveSkillCondition(List<bool> skillList)
    {
        data.UsePassiveList = skillList;
    }

    public static void Save()
    {
        string json = JsonUtility.ToJson(data);
    #if UNITY_EDITOR
        string path = Directory.GetCurrentDirectory();
    #else
        string path = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\');
    #endif
        path += ("/" + DATA_FILE_PATH);
        StreamWriter writer = new StreamWriter(path,false);
        writer.WriteLine(json);
        writer.Flush();
        writer.Close();
    }

    public static void Load()
    {
        try
        {
        #if UNITY_EDITOR
            string path = Directory.GetCurrentDirectory();
        #else
            string path = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\');
        #endif
            FileInfo info = new FileInfo(path + "/" + DATA_FILE_PATH);
            StreamReader reader = new StreamReader(info.OpenRead());
            string json = reader.ReadToEnd();
            data = JsonUtility.FromJson<SaveData>(json);
        }
        catch(Exception e)
        {
            Debug.LogWarning(e);
            data = new SaveData();
        }
    }
}
