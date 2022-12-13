using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �G�t�F�N�g�̕\���^�C�~���O�𒲐�����
/// </summary>
public class EffectDelayDisplayer : MonoBehaviour
{
    [SerializeField]
    [Tooltip("����������G�t�F�N�g")]
    GameObject _effect = default;
    [SerializeField]
    [Tooltip("������x�点�鎞��")]
    float _delayTime = default;

    /// <summary>�G�t�F�N�g�̔j���܂ł̑ҋ@����</summary>
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
