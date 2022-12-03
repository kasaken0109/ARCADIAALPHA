using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwordInformationDisplay : MonoBehaviour
{
    [System.Serializable]
    class SwordDisplays
    {
        public Text Name;
        public Text Explanation;
        public Image Image;
    }
    [SerializeField]
    SwordDisplays[] _swordDisplays;
    [SerializeField]
    SwordInformation[] _swordInformations;
    // Start is called before the first frame update
    void Start()
    {
        SetInformation();
    }

    void SetInformation()
    {
        for (int i = 0; i < _swordDisplays.Length; i++)
        {
            _swordDisplays[i].Name.text = "s" + _swordInformations[i].Name + "t";
            var text = _swordInformations[i].Explanation;
            if (text.Contains("\\n"))
            {
                text = text.Replace(@"\n", Environment.NewLine);
            }
            _swordDisplays[i].Explanation.text = text;
            _swordDisplays[i].Image.sprite = _swordInformations[i].Image;
        }
    }
}
