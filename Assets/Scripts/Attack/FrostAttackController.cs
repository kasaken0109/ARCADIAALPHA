using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostAttackController : MonoBehaviour,IDamage
{
    [SerializeField]
    [Tooltip("攻撃力")]
    float m_damage = 100f;

    [SerializeField]
    [Tooltip("")]
    GameObject m_effect = default;

    ParticleSystem particle;
    float duraration;
    float time;

    float damage;
    public int Damage => Mathf.CeilToInt(damage);
    // Start is called before the first frame update
    void Start()
    {
        particle = GetComponent<ParticleSystem>();
        duraration = particle.main.duration;
        time = particle.main.duration;
        StartCoroutine(nameof(SetDamage));
    }

    IEnumerator SetDamage()
    {
        while (time > 0)
        {
            damage = m_damage * (duraration - particle.time) / duraration;
            yield return new WaitForSeconds(1f);
            time--;
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        if (damage < m_damage * 0.1f) return;
        var obj = Instantiate(m_effect,transform.position,Quaternion.identity);
        //float size = 0.01f * damage / m_damage;
        //obj.transform.localScale = new Vector3(size, size, size);
    }

    public void AddDamage(int damage,ref GameObject call)
    {
        GetComponentInParent<IDamage>().AddDamage(damage, ref call);
        Destroy(gameObject);
    }
}
