using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarriaDisplay : MonoBehaviour,IDamage
{
    [SerializeField]
    int _barriaHp = 2;
    [SerializeField]
    Transform _chasePoint = default;

    int hp = 0;

    public void AddDamage(int damage, ref GameObject call)
    {
        if(hp > damage)
        {
            hp -= damage;
        }
        else
        {
            hp = 0;
            gameObject.SetActive(false);
        }
    }

    // Start is called before the first frame update
    private void OnEnable()
    {
        hp = _barriaHp;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = _chasePoint.position;
    }
}
