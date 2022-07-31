using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ブレス攻撃の挙動を制御する
/// </summary>
public class BreathController : MonoBehaviour
{
    [SerializeField]
    private float m_speed;
    [SerializeField]
    private string m_name = "EnemyBoss";

    Rigidbody m_rb;
    // Start is called before the first frame update
    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
        m_rb.velocity = GameObject.Find(m_name).transform.forward * m_speed;
        gameObject.SetActive(true);
    }
}
