using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyScriptable/Create MotionData")]
class Motion : ScriptableObject
{
    [SerializeField] int m_attackPower;
    [SerializeField] AnimationClip motionClip;
}
