using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

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
                //instance.Init();
                DontDestroyOnLoad(instance);
            }
            return instance;
        }
    }

    public GameObject CurrentSword => _currentSword;

    public int Point { get; set; }

    GameObject _currentSword = default;

    void Init()
    {
        //ÉfÅ[É^Çälìæ
        _bulletReactiveCollection.AddTo(this);

    }
    public void SetSword(GameObject sword)
    {
        _currentSword = sword;
    }
    public Bullet[] Equipments {
        get
        {
            for (int i = 0; i < _equipments.Length; i++)
            {
                _equipments[i] = _equipments[i] ? _equipments[i] : ServiceLocator.GetInstance<EquipDataPresenter>().GetInitBullet(i);
            }
            return _equipments;
        }
    }

    ReactiveCollection<Bullet> _bulletReactiveCollection = new ReactiveCollection<Bullet>();
    Bullet[] _equipments = new Bullet[3];

    private int _equipID = 1;

    public int GetEquipID => _equipID;

    public void SetEquipments(Bullet bullet)
    {
        Equipments[_equipID] = bullet;
        //_bulletReactiveCollection[_equipID] = bullet;
    }

    public void SetSkill(CustomSkill skill)
    {
        var equip = Equipments[_equipID];
        equip.passiveSkill = skill;
    }

    public void SetEquipID(int value) => _equipID = value;

    private void OnDestroy()
    {
        
    }

}
