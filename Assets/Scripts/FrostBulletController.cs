using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostBulletController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("敵に着弾時に発生する")]
    private GameObject m_freeze = null;

    [SerializeField]
    [Tooltip("地面に着弾時に発生する")]
    private GameObject m_explosion = null;

    [SerializeField]
    private int m_attackpower = 10;

    private　bool IsCreateWall;

    Vector3 hitPos;
    // Start is called before the first frame update
    void Start()
    {
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Floor")
        {
            Destroy(this.gameObject);

            foreach (ContactPoint point in collision.contacts)
            {
                hitPos = point.point;
            }
            IsCreateWall = true;
        }
        else if (collision.collider.tag == "Enemy" || collision.collider.tag == "Item")
        {
            collision.gameObject.GetComponentInParent<IDamage>().AddDamage(m_attackpower,ref m_freeze);
            foreach (ContactPoint point in collision.contacts) hitPos = point.point;
            IsCreateWall = false;
            Destroy(this.gameObject);
        }
    }

    private void OnDestroy()
    {
        if (!m_freeze) return;

        var obj  = Instantiate(IsCreateWall ? m_freeze : m_explosion);
        obj.transform.position = new Vector3(hitPos.x , -1.38f, hitPos.z);
        Quaternion look = Quaternion.FromToRotation(transform.forward, transform.forward + transform.up);//Quaternion.AngleAxis(0,transform.forward + transform.up);
        obj.transform.rotation = look;
        SoundManager.Instance.PlayFrost();
    }
}
