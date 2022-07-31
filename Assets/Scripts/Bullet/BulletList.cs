using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BulletList")]
public class BulletList : ScriptableObject
{
    [SerializeField]
    private List<Bullet> _bullets = default;

    public List<Bullet> Bullets => _bullets;
}