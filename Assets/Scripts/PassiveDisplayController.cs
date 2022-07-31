using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PassiveDisplayController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("エフェクトの発生ポイント")]
    Transform _effectBirthPoint = default;

    [SerializeField]
    [Tooltip("")]
    Image[] _effectDisplays;

    [SerializeField]
    Sprite _defaultImage;

    public void SetDisplay(ref CustomSkill passiveSkill)
    {
        var obj = Instantiate(passiveSkill.Effect, _effectBirthPoint.position, _effectBirthPoint.rotation, _effectBirthPoint);
        var particle = obj.GetComponent<ParticleSystem>().main;
        //particle.duration = passiveSkill.EffectableTime;
    }
}
