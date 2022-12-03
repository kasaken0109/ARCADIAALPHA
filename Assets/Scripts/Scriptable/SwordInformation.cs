using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "SwordInformation")]
public class SwordInformation : ScriptableObject
{
    [SerializeField]
    string _name = "�f�t�H���g";
    [SerializeField]
    string _explanation�@= "���ݒ�";
    [SerializeField]
    Sprite _image;

    public string Name => _name;

    public string Explanation => _explanation;

    public Sprite Image => _image;
}
