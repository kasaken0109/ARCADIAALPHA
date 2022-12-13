using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UnlockController : MonoBehaviour
{
    [SerializeField]
    Button _firstSelected = default;
    [SerializeField]
    Text _unlockBulletNameHolder = default;
    [SerializeField]
    Image _unlockBulletImageHolder = default;
    [SerializeField]
    Text _unlockPointHolder = default;

    GameObject preSelected = null;
    
    private void OnEnable()
    {
        preSelected = EventSystem.current.currentSelectedGameObject;
        //EquipmentManagerから選択した弾の情報を取得
        EventSystem.current.SetSelectedGameObject(_firstSelected.gameObject);
        SetUnlockInformation();
    }

    private void OnDisable()
    {
        EventSystem.current.SetSelectedGameObject(preSelected);
    }

    void SetUnlockInformation()
    {
        var unlockBullet = EquipmentManager.Instance.GetCurrentSelectedBullet();
        _unlockBulletImageHolder.sprite = unlockBullet.Image;
        _unlockBulletNameHolder.text = unlockBullet.Name;
        _unlockPointHolder.text = "現在のポイント : " + EquipmentManager.Instance.Point;
    }

    public void Unlock()
    {
        var bullet = EquipmentManager.Instance.GetCurrentSelectedBullet();
        bullet.IsUnlock = true;
        ServiceLocator.GetInstance<EquipDataPresenter>().SetUnlock(bullet.BulletID);
        EquipmentManager.Instance.Point -= bullet.RequirePointToUnlock;
        gameObject.SetActive(false);
    }


}
