using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "SwordInformation")]
public class SwordInformation : ScriptableObject
{
    [SerializeField]
    string _name = "デフォルト";
    [SerializeField]
    string _explanation　= "未設定";
    [SerializeField]
    Sprite _image;

    public string Name => _name;

    public string Explanation => _explanation;

    public Sprite Image => _image;
}
