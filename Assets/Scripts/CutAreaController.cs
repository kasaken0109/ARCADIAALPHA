using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutAreaController : MonoBehaviour
{
    [SerializeField] float m_speed = 0.2f;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float inputh = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector3(inputh * m_speed, 0, 0);
        //rb.AddForce(new Vector3(inputh * m_speed,0,0),ForceMode.Force);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CutAble"))
        {
            other.tag = "Cut";
            other.GetComponent<Renderer>().material.color = Color.red;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Cut"))
        {
            other.tag = "CutAble";
            other.GetComponent<Renderer>().material.color = Color.white;
        }
    }
}
