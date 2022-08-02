#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BLINDED_AM_ME;
using UnityEngine.UI;

public class CutController : MonoBehaviour
{
    [SerializeField] 
    GameObject[] m_cutObjects = null;
    [SerializeField]
    GameObject m_cutplane = null;
    [SerializeField]
    Material m_cutMaterial = null;
    [SerializeField]
    LineRenderer lineRenderer;
    Vector3 worldPos;
    Vector3 pos;
    public bool m_cutMode = true;
    MeshCut meshCut;
    MeshFilter plane;
    GameObject[] cutObjects;
    Vector3[] col;
    // Start is called before the first frame update
    void Start()
    {
        plane = m_cutplane.GetComponent<MeshFilter>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(lineRenderer);
        pos = Input.mousePosition;
        m_cutObjects = GameObject.FindGameObjectsWithTag("Cut");
    }

    public void CutObject(bool cutMode)
    {
        Debug.Log(m_cutplane.transform.rotation.z);
        Vector3 cut = Vector3.zero;
        if (cutMode)
        {
            //cut = new Vector3(1, m_cutplane.transform.rotation.z, 0);
            cut = new Vector3(1, m_cutplane.transform.rotation.z, m_cutplane.transform.rotation.y);
            //cut = m_cutplane.transform.rotation.eulerAngles.;
        }
        else
        {
            if (lineRenderer.GetPosition(1) != null)
            {
                cut = new Vector3((lineRenderer.GetPosition(1) - lineRenderer.GetPosition(0)).y, -(lineRenderer.GetPosition(1) - lineRenderer.GetPosition(0)).x, 0);
            }
        }
        if (m_cutObjects == null) return;
        foreach (var item in m_cutObjects)
        {
            //cutObjects = MeshCut.Cut(item, item.transform.position, cut, m_cutMaterial);
            cutObjects = MeshCut.Cut(item, m_cutplane.transform.position, cut, m_cutMaterial);
            //new Vector3(worldPos.x, worldPos.y, 0)
        }
        if (cutObjects == null) return;
        foreach (var item in cutObjects)
        {   if(!item.TryGetComponent(out Rigidbody rb))
            {
                Debug.Log("addrig");
                rb = item.AddComponent<Rigidbody>();
                rb.mass = 1;
            }
            else
            {
                item.TryGetComponent(out Rigidbody rb2);
                rb2.useGravity = true;
            }
            if (item.TryGetComponent(out BoxCollider col))
            {
                col.enabled = false;
            }
            if (!item.TryGetComponent(out MeshCollider u))
            {
                u = item.AddComponent<MeshCollider>();
                u.convex = true;
            }
            else
            {
                Destroy(u);
                var v = item.AddComponent<MeshCollider>();
                v.convex = true;
            }
            //rb.AddForce(new Vector3((lineRenderer.GetPosition(1) - lineRenderer.GetPosition(0)).x, (lineRenderer.GetPosition(1) - lineRenderer.GetPosition(0)).y, 0).normalized * m_cutPower,ForceMode.Force);
        }
    }

    public void OnBeginDrag()
    {
        pos.z = 1;
        worldPos = Camera.main.ScreenToWorldPoint(pos);
        //if (!IsFirst)
        //{
        //    lineRenderer.SetPosition(0, Vector3.zero);
        //    lineRenderer.SetPosition(1, Vector3.zero);
        //}
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0,new Vector3(worldPos.x, worldPos.y,1));
        //lineRenderer.SetPosition(0, new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1));
    }

    public void OnEndDrag()
    {
        pos.z = 1;
        worldPos = Camera.main.ScreenToWorldPoint(pos);
        lineRenderer.SetPosition(1, new Vector3(worldPos.x, worldPos.y, 1));
        //lineRenderer.SetPosition(1, new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1));
        CutObject(m_cutMode);
    }
}
#endif