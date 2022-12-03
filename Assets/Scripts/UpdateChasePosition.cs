using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateChasePosition : MonoBehaviour
{
    [SerializeField]
    Transform _chase = default;
    // Start is called before the first frame update
    // Update is called once per frame
    void Update()
    {
        //transform.position = _chase.position;
    }

    private void FixedUpdate()
    {
        transform.position = _chase.position;
    }
}
