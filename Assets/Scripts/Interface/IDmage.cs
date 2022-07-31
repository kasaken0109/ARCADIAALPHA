using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IDamage
{
    void AddDamage(int damage,ref GameObject call);
}

interface IGetDamage
{
    void GetDamage(int damage);
}
