using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="chatGroup")]
public class ChatGroup : ScriptableObject
{
    public ChatData[] chatDatas = default;
}
