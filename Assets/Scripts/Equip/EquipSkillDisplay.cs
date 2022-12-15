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
    [SerializeField]
    GroupUIAnim[] groupBuffUIActiveAnims = default;
    [SerializeField]
    GroupUIAnim[] groupBuffUINonActiveAnims = default;
    [SerializeField]
    GroupUIAnim[] groupDebuffUIActiveAnims = default;
    [SerializeField]
    GroupUIAnim[] groupDebuffUINonActiveAnims = default;
    EventSystem eventSystem;
    float upperColumPos;
    float lowerColumPos;

    private void Awake()
    {
        ServiceLocator.SetInstance(this);
        upperColumPos = _buff[0].transform.position.y;
        lowerColumPos = _debuff[0].transform.position.y;
    }
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

    public void SetSkillDisplay()
    {
        var locator = ServiceLocator.GetInstance<UIGroupAnimationController>();
        if (!EquipmentManager.Instance.Equipments[EquipmentManager.Instance.GetEquipID]) return;
        var bullet = EquipmentManager.Instance.Equipments[EquipmentManager.Instance.GetEquipID];
        switch (bullet.BulletCustomType)
        {
            case BulletCustomType.Buff:
                _all.ToList().ForEach(c => c.SetActive(false));
                _buff.ToList().ForEach(c => c.SetActive(true));
                locator.groupUIActiveSetAnims = groupBuffUIActiveAnims;
                locator.groupUINonActiveSetAnims = groupBuffUINonActiveAnims;

                break;
            case BulletCustomType.Debuff:
                _all.ToList().ForEach(c => c.SetActive(false));
                _debuff.ToList().ForEach(c => { c.SetActive(true);c.GetComponent<MovableUI>().SetDelta(true); });
                locator.groupUIActiveSetAnims = groupDebuffUIActiveAnims;
                locator.groupUINonActiveSetAnims = groupDebuffUINonActiveAnims;
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

    public void Reset()
    {
        _all.ToList().ForEach(c => c.SetActive(false));
        _debuff.ToList().ForEach(c => c.GetComponent<MovableUI>().SetDelta(false));
        _all.ToList().ForEach(c => c.SetActive(true));
    }

    private void OnDestroy()
    {
        ServiceLocator.RemoveInstance<EquipSkillDisplay>();
    }
}
