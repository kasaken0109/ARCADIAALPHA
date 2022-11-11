using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    private static EquipmentManager instance = null;

    public static EquipmentManager Instance
    {
        get
        {
            var target = FindObjectOfType<EquipmentManager>();
            if (target)
            {
                instance = target;
            }
            else
            {
               var gm = GameObject.Find("GM");
                if (!gm) gm = new GameObject("GM");
                instance = gm.AddComponent<EquipmentManager>();
                DontDestroyOnLoad(instance);
            }
            return instance;
        }
    }

    public GameObject CurrentSword => _currentSword;

    GameObject _currentSword = default;
    public void SetSword(GameObject sword)
    {
        _currentSword = sword;
    }

    [HideInInspector]
    public Bullet[] Equipments = new Bullet[3];

    private int _equipID = 1;

    public int GetEquipID => _equipID;

    public void SetEquipments(Bullet bullet)
    {
        Equipments[_equipID] = bullet;
    }

    public void SetSkill(CustomSkill skill)
    {
        var equip = Equipments[_equipID];
        equip.passiveSkill = skill;
    }

    public void SetEquipID(int value) => _equipID = value;

}
