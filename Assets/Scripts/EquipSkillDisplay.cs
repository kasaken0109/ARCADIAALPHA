using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EquipSkillDisplay : MonoBehaviour
{
    [SerializeField]
    GameObject[] _all = default;
    [SerializeField]
    GameObject[] _buff = default;
    [SerializeField]
    GameObject[] _debuff = default;
    EventSystem eventSystem;
    // Start is called before the first frame update
    void OnEnable()
    {
        eventSystem = FindObjectOfType<EventSystem>();
        SetSkillDisplay();
    }

    private void OnDisable()
    {
        Reset();
    }

    void SetSkillDisplay()
    {
        if (!EquipmentManager.Instance.Equipments[EquipmentManager.Instance.GetEquipID]) return;
        var bullet = EquipmentManager.Instance.Equipments[EquipmentManager.Instance.GetEquipID];
        switch (bullet.BulletCustomType)
        {
            case BulletCustomType.Buff:
                _all.ToList().ForEach(c => c.SetActive(false));
                _buff.ToList().ForEach(c => c.SetActive(true));
                break;
            case BulletCustomType.Debuff:
                _all.ToList().ForEach(c => c.SetActive(false));
                _debuff.ToList().ForEach(c => c.SetActive(true));
                IEnumerator Wait()
                {
                    yield return new WaitForSeconds(0.3f);
                    eventSystem.SetSelectedGameObject(_debuff[0].GetComponentInChildren<Selectable>().gameObject);
                }
                StartCoroutine(Wait());
                break;
            case BulletCustomType.All:
                break;
            default:
                break;
        }
    }

    private void Reset()
    {
        _all.ToList().ForEach(c => c.SetActive(false));
        _all.ToList().ForEach(c => c.SetActive(true));
    }
}
