using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipSwordController : MonoBehaviour
{
    [SerializeField]
    GameObject[] _swords;
    [SerializeField]
    GameObject[] _swordsEquip;
    [SerializeField]
    Image _currentImage = default;
    [SerializeField]
    SwordInformation[] _swordInformations = default;
    [SerializeField]
    Transform _dispalyParentPos = default;

    GameObject _currentSword = null;
    int id = 0;

    private void Awake()
    {
        ServiceLocator.SetInstance<EquipSwordController>(this);
    }
    public void SetWeapon(bool isPrev)
    {
        id = isPrev ?
                (id == 0 ? _swords.Length - 1 : id -= 1) :
                (id == _swords.Length - 1 ? 0 : id += 1);
        
    }

    public void EquipSwordWeapon()
    {
        for (int i = 0; i < _swordsEquip.Length; i++)
        {
            _swordsEquip[i].SetActive(i == id);
        }
        EquipmentManager.Instance.SetSword(_swords[id]);
        if (_currentSword) Destroy(_currentSword);
        _currentSword = Instantiate(_swords[id], _dispalyParentPos);
        _currentImage.sprite = _swordInformations[id].Image;
    }

    private void OnDestroy()
    {
        ServiceLocator.RemoveInstance<EquipSwordController>();
    }
}
