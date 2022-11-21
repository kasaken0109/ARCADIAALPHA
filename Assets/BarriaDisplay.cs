using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BarriaDisplay : MonoBehaviour,IDamage
{
    [SerializeField]
    int _barriaHp = 2;
    [SerializeField]
    Transform _chasePoint = default;
    [SerializeField]
    GameObject _damage;
    [SerializeField]
    GameObject _fragileEffect = default;
    [SerializeField]
    GameObject _crush;
    [SerializeField]
    UnityEvent _hitEvent;
    [SerializeField]
    Material _damaged = default;

    Renderer renderer = default;
    int hp = 0;

    public void AddDamage(int damage, ref GameObject call)
    {
        if(hp > damage)
        {
            hp -= damage;
            Instantiate(_damage, transform.position, transform.rotation);
            _fragileEffect.SetActive(hp == 1);
        }
        else
        {
            hp = 0;
            Instantiate(_crush,transform.position,transform.rotation);
            gameObject.SetActive(false);
        }
        _hitEvent?.Invoke();
    }

    private void Start()
    {
        renderer = GetComponentInChildren<Renderer>();
    }
    // Start is called before the first frame update
    private void OnEnable()
    {
        hp = _barriaHp;
    }
    private void OnDisable()
    {
        _fragileEffect.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = _chasePoint.position;
    }
}
