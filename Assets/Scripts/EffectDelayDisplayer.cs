using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDelayDisplayer : MonoBehaviour
{
    [SerializeField]
    GameObject _effect = default;
    [SerializeField]
    float _delayTime = default;

    public float DelayDestroy { set; get; }
    // Start is called before the first frame update
    void Start()
    {
        IEnumerator Spawn()
        {
            yield return new WaitForSeconds(_delayTime);
            var m = Instantiate(_effect,transform);
            yield return new WaitForSeconds(DelayDestroy);
            Destroy(gameObject);
        }
        StartCoroutine(Spawn());
    }
}
