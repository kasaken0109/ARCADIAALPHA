using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChraractorDisplayController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("キャラクターの回転速度")]
    float _rotateSpeed = 0.1f;
    // Start is called before the first frame update
    void FixedUpdate()
    {
        //var angle = Input.GetAxisRaw("Horizontal");
        //DisplayRotate(-angle);
    }

    void DisplayRotate(float speed)
    {
        var angle = transform.rotation * Quaternion.AngleAxis(speed, Vector3.up);
        transform.rotation = angle;
    }
}
