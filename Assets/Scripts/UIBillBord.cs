using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBillBord : MonoBehaviour
{
    [SerializeField]
    bool IsUpdate = false;
    // Start is called before the first frame update
    void Start()
    {
        var rotation = transform.rotation * Quaternion.Euler(0, Camera.main.transform.rotation.y, 0);// * Quaternion.Euler(0, 180, 0);/*transform.rotation * Camera.main.transform.rotation * Quaternion.Euler(0,180,0);*/
        transform.rotation = rotation;
    }

    private void Update()
    {
        if (IsUpdate)
        {
            //var rotation = transform.rotation * Quaternion.Euler(0, Camera.main.transform.rotation.y, 0);// * Quaternion.Euler(0, 180, 0);/*transform.rotation * Camera.main.transform.rotation * Quaternion.Euler(0,180,0);*/
            transform.LookAt(Camera.main.transform);
        }
    }
}
