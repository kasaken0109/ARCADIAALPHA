using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// エフェクトの表示タイミングを調整する
/// </summary>
public class EffectDelayDisplayer : MonoBehaviour
{
    [SerializeField]
    [Tooltip("発生させるエフェクト")]
    GameObject _effect = default;
    [SerializeField]
    [Tooltip("発生を遅らせる時間")]
    float _delayTime = default;

    /// <summary>エフェクトの破棄までの待機時間</summary>
    public float DelayDestroy { set; get; }
    // Start is called before the first frame update
    void Start()
    {
        IEnumerator Spawn()
        {
            yield return new WaitForSeconds(_delayTime);
            var m = Instantiate(_effect,transform);
            Destroy(gameObject, DelayDestroy);
        }
        StartCoroutine(Spawn());
    }
}
