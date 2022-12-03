using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipBulletDisplay : MonoBehaviour
{
    [SerializeField]
    int _bulletID = 0;
    [SerializeField]
    Image _coolDown = default;
    [SerializeField]
    Image _coolDownBackground = default;
    [SerializeField]
    Image _frameFront = default;
    [SerializeField]
    Image _skill = default;
    [SerializeField]
    Sprite _skillNonAssigned = default;
    [SerializeField]
    Color _disable;
    private void OnEnable()
    {
        //Active();
    }
    public void Active()
    {
        _frameFront.gameObject.SetActive(true);
        _coolDown.color = Color.white;
        _skill.color = Color.white;
    }
    public void NonActive()
    {
        _frameFront.gameObject.SetActive(false);
        _coolDown.color = new Color(_coolDown.color.r, _coolDown.color.g, _coolDown.color.b, 0);
        _skill.color = _disable;
    }
    public void CoolDown(float time)
    {
        StartCoroutine(DisplayCoolDown(time));
        IEnumerator DisplayCoolDown(float time)
        {
            _coolDown.fillAmount = 0;
            while (_coolDown.fillAmount < 0.99f)
            {
                _coolDown.fillAmount += 1.0f * Time.deltaTime / time;
                yield return null;
            }
        }
    }

    public void SetBulletInformation()
    {
        var bullet = EquipmentManager.Instance.Equipments[_bulletID];
        _coolDown.sprite = bullet.EquipImage;
        _coolDownBackground.sprite = bullet.EquipCoolDownImage;
        _skill.sprite = bullet.PassiveSkill == null ? _skillNonAssigned : bullet.PassiveSkill.ImageBullet;
    }
}
