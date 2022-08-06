using UnityEngine;
using UnityEngine.Rendering;

[ExecuteAlways]
public class FrostEffectController : MonoBehaviour
{
    public VolumeProfile volumeProfile; // プロジェクトに作ったPostProcessVolume Profileをアタッチします
    //[Range(0, 1)]
    [Tooltip("画面に表示する氷の量")]
    public FloatParameter frostAmount = new FloatParameter(0.5f);

    //[Range(0, 10)]
    [Tooltip("氷の硬さの表示")]
    public FloatParameter edgeSharpness = new FloatParameter(1f);

    [Tooltip("表示する氷の最小量")]
    public FloatParameter minFrost = new FloatParameter(0);

    [Tooltip("表示する氷の最大量")]
    public FloatParameter maxFrost = new FloatParameter(1f);

    [Tooltip("視界の見易さ")]
    public FloatParameter seethroughness = new FloatParameter(0.2f);

    [Tooltip("散らばり具合")]
    public FloatParameter distortion = new FloatParameter(0.1f);

    public Texture2D Frost; //RGBA
    public Texture2D FrostNormals; //normalmap
    FrostEffect frostEffect;

    void Update()
    {
        //if (volumeProfile == null) return;
        //if (frostEffect == null) volumeProfile.TryGet<FrostEffect>(out frostEffect);
        //if (frostEffect == null) return;

        //frostEffect.frostAmount.value = frostAmount.value;
        //frostEffect.edgeSharpness.value = edgeSharpness.value;
        //frostEffect.minFrost.value = minFrost.value;
        //frostEffect.maxFrost.value = maxFrost.value;
        //frostEffect.seethroughness.value = seethroughness.value;
        //frostEffect.distortion.value = distortion.value;
        //frostEffect.Frost = Frost;
        //frostEffect.FrostNormals = FrostNormals;
    }
}
