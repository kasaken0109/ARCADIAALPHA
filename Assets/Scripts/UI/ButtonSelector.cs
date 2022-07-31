using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonSelector : MonoBehaviour,IDeselectHandler,ISelectHandler
{
    [SerializeField]
    private GameObject _checkMark = default;
    [SerializeField]
    private Image _bulletImage = default;
    [SerializeField]
    private Text _bulletName = default;

    Button _button;
    Bullet _set;
    CustomSkill _skill;

    public int ID { get; set; }

    public Bullet Set => _set;

    public void OnDeselect(BaseEventData eventData)
    {
        _checkMark.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        _button = GetComponent<Button>();
        _checkMark.SetActive(false);
    }

    public void SetInformation(Bullet bullet)
    {
        _bulletImage.sprite = bullet.Image;
        _bulletName.text = bullet.Name;
        _set = bullet;
        if(!_button) _button = GetComponent<Button>();
       // _button.onClick.AddListener(() => InformationPresenter.Instance.SetExplanation(_set));
        _button.onClick.AddListener(() => EquipmentManager.Instance.SetEquipments(_set));
    }

    public void SetInformation(CustomSkill skill)
    {
        _bulletImage.sprite = skill.ImageBullet;
        _bulletName.text = skill.SkillName;
        _skill = skill;
        if (!_button) _button = GetComponent<Button>();
        //_button.onClick.AddListener(() => InformationPresenter.Instance.SetExplanation(_skill));
        _button.onClick.AddListener(() => EquipmentManager.Instance.SetSkill(_skill));
    }

    public void SetBullet()
    {
        EquipmentManager.Instance.SetEquipments(_set);
    }

    public void OnSelect(BaseEventData eventData)
    {
        InformationPresenter.Instance.SetExplanation(_set);
        InformationPresenter.Instance.SetExplanation(_skill);
    }
}
