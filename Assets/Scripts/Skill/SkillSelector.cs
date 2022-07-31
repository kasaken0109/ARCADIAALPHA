using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSelector : MonoBehaviour
{
    [SerializeField]
    private SkillList _list = default;
    [SerializeField]
    private ButtonSelector[] _buttons = default;

    // Start is called before the first frame update
    void Start()
    {
        BulletInformationInit();
    }

    public void BulletInformationInit()
    {
        for (int i = 0; i < _list.Skills.Count; i++)
        {
            _buttons[i].SetInformation(_list.Skills[i]);
        }
    }
}
