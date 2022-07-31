using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 破壊可能なオブジェクトを管理する
/// </summary>
public class EffectObjectController : MonoBehaviour,IDamage
{
    [SerializeField]
    [Tooltip("発生するエフェクト")]
    GameObject m_effect;
    [SerializeField]
    GameObject m_item;
    [SerializeField]
    GameObject m_mine;
    public void AddDamage(int damage,ref GameObject call)
    {
        StartCoroutine(nameof(InstanceObj));
    }

    IEnumerator InstanceObj()
    {
        
        Instantiate(m_effect, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.2f);
        Instantiate(m_item, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
