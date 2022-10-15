using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LayserModuleController : MonoBehaviour
{
    [SerializeField]
    private GameObject m_effect = null;

    [SerializeField]
    [Tooltip("射程距離")]
    private float m_shootRange = 15f;

    [SerializeField]
    [Tooltip("当たるレイヤー")]
    private LayerMask m_layerMask = 0;

    [SerializeField]
    private AudioClip m_hitSound = null;

    bool IsSounded = false;
    bool IsHitSound = false;

    int damage;
    public int Damage { set { damage = value; } }
    Vector3 boxScale = new Vector3(10, 10, 2);
    Vector3 hitPosition;
    Ray ray;
    GameObject hitObject = null;

    GameObject enemy;
    // Start is called before the first frame update
    void Start()
    {
        enemy = GameObject.FindWithTag("Enemy");
        LookEnemy();
    }

    private void LookEnemy()
    {
        if (!enemy) return;
        transform.LookAt(new Vector3(enemy.transform.position.x, transform.position.y, enemy.transform.position.z));
    }

    private void Update()
    {
        if (CanUse)
        {
            RayHit(ray, ref hitObject);
            StartCoroutine(nameof(WaitCoolDown));
        }
        LookEnemy();
    }

    bool CanUse = true; 
    IEnumerator WaitCoolDown()
    {
        CanUse = false;
        yield return new WaitForSeconds(0.5f);
        CanUse = true;
    }

    bool IsEnd = false;
    private RaycastHit RayHit(Ray ray, ref GameObject hitObject)
    {
        RaycastHit hit;
        bool IsHit = Physics.BoxCast(gameObject.transform.position, boxScale, transform.forward, out hit, gameObject.transform.rotation, m_shootRange);

        if (IsHit)
        {
            hitPosition = hit.point;    // Ray が当たった場所
            hitObject = hit.collider.gameObject;    // Ray が洗ったオブジェクト

            if (hitObject)
            {
                if (hitObject.tag == "Enemy")
                {
                    IsSounded = !IsSounded ? true : false;
                    hitObject.GetComponentInParent<IDamage>().AddDamage(damage,ref m_effect);
                    if(!IsEnd)Instantiate(m_effect, hitPosition, Quaternion.identity);
                }
                if (!IsHitSound)
                {
                    PlayHitSound(hitPosition);  // レーザーが当たった場所でヒット音を鳴らす
                    SoundManager.Instance.PlayFrost();
                    IsHitSound = true;
                }
                IsEnd = true;
            }
        }
        return hit;
    }

    /// <summary>
    /// ヒット音を鳴らす
    /// </summary>
    /// <param name="position">音を鳴らす場所</param>
    void PlayHitSound(Vector3 position)
    {
        if (m_hitSound) AudioSource.PlayClipAtPoint(m_hitSound, position, 0.1f);
    }
}
