using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "MyScriptable/Create QuestData")]
public class Quest : ScriptableObject
{
    [SerializeField] Color m_color;
    [SerializeField] string m_loadSceneName = null;
    [SerializeField] string m_questName = null;
    [SerializeField] string m_questText = null;
    [SerializeField] Sprite m_image = null;
    [SerializeField] Sprite m_backGround = null;
    public string LoadSceneName() { return m_loadSceneName; }
    public string QuestName() { return m_questName; }
    public string QuestText() { return m_questText; }

    public Sprite QuestImage() { return m_image; }

    public Sprite QuestBackGroundImage() { return m_backGround; }
    public Color QuestColor() { return m_color; }

    // Start is called before the first frame update
}
