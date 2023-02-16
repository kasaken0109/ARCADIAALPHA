using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class EquipSwordController : MonoBehaviour
{
    [SerializeField]
    GameObject[] _swords;
    [SerializeField]
    GameObject[] _swordsEquip;
    [SerializeField]
    Sprite[] _displayImages = default;
    [SerializeField]
    Image _currentImage = default;
    [SerializeField]
    SwordInformation[] _swordInformations = default;
    [SerializeField]
    Transform _dispalyParentPos = default;
    [SerializeField]
    EquipButtonSelect _buttonSelect = default;

    GameObject _currentSword = null;
    int id = 0;

    private void Awake()
    {
        ServiceLocator.SetInstance(this);
    }

    private void Start()
    {
        _buttonSelect.IsSelect.Subscribe(x => SetHighlightImage(x)).AddTo(this);
        EquipSwordWeapon();
    }

    void SetHighlightImage(bool isHighlight)
    {
        _currentImage.sprite = _displayImages[id + (isHighlight ? _displayImages.Length / 2 : 0)];
    }
    public void SetWeapon(bool isPrev)
    {
        SoundManager.Instance.PlayClick(5);
        id = isPrev ?
                (id == 0 ? _swords.Length - 1 : id -= 1) :
                (id == _swords.Length - 1 ? 0 : id += 1);
        
    }

    public void EquipSwordWeapon()
    {
        SoundManager.Instance.PlayClick(6);
        var m = FindObjectOfType<TutorialEquipMissionViewModel>();
        for (int i = 0; i < _swordsEquip.Length; i++)
        {
            _swordsEquip[i].SetActive(i == id);
        }
        EquipmentManager.Instance.SetSword(_swords[id]);
        if (_currentSword) Destroy(_currentSword);
        _currentSword = Instantiate(_swords[id], _dispalyParentPos);
        _currentImage.sprite = _displayImages[id];
        //if (m && m.GetValue == 7)
        //{
        //    FindObjectOfType<TutorialEquipMissionViewModel>().SetSwordEquip();
        //}
    }

    private void OnDestroy()
    {
        ServiceLocator.RemoveInstance<EquipSwordController>();
    }
}
