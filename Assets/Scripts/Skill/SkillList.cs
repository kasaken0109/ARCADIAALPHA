using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SkillList")]
public class SkillList : ScriptableObject
{
    [SerializeField]
    private List<CustomSkill> _skills = default;

    public List<CustomSkill> Skills => _skills;
}

