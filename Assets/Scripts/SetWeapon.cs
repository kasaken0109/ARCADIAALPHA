using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SetWeapon : MonoBehaviour
{
    [SerializeField]
    Transform _parentPoint = default;
    [SerializeField]
    GameObject _default = default;
    AttackcolliderController[] _attackcolliderControllers = default;

    public AttackcolliderController[] AttackcolliderControllers => _attackcolliderControllers;
    // Start is called before the first frame update
    void Awake()
    {
        var m =Instantiate(EquipmentManager.Instance.CurrentSword ? EquipmentManager.Instance.CurrentSword : _default, _parentPoint);
        _attackcolliderControllers = m.GetComponentsInChildren<AttackcolliderController>();
        for (int i = 0; i < _attackcolliderControllers.Length; i++)
        {
            _attackcolliderControllers[i].gameObject.SetActive(false);
        }
        GetComponentInParent<AttackSetController>().AttackCollider = _attackcolliderControllers.OrderBy(x => x.ID).ToList();
    }
}
