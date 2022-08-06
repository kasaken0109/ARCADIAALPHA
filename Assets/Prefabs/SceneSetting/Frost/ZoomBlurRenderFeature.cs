using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

//public class ZoomBlurRenderFeature : ScriptableRendererFeature
//{
    //FrostEffectPass zoomBlurPass;

    //public override void Create()
    //{
    //    zoomBlurPass = new FrostEffectPass(RenderPassEvent.BeforeRenderingPostProcessing);
    //}

    //public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    //{
    //    zoomBlurPass.Setup(renderer.cameraColorTarget);
    //    renderer.EnqueuePass(zoomBlurPass);
    //}
//}

//public class FrostEffectPass : ScriptableRenderPass
//{
    //static readonly string k_RenderTag = "Render ZoomBlur Effects";
    //static readonly int MainTexId = Shader.PropertyToID("_MainTex");
    //static readonly int TempTargetId = Shader.PropertyToID("_TempTargetZoomBlur");
    //static readonly int BlendTexId = Shader.PropertyToID("_BlendTex");
    //static readonly int BlendAmountId = Shader.PropertyToID("_BlendAmount");
    //static readonly int EdgeSharpnessId = Shader.PropertyToID("_EdgeSharpness");
    //static readonly int BumpMapId = Shader.PropertyToID("_BumpMap");
    //static readonly int SeeThroughnessId = Shader.PropertyToID("_SeeThroughness");
    //static readonly int DistortionId = Shader.PropertyToID("_Distortion");
    //FrostEffect frostEffect;
    //Material frostEffectMaterial;
    //RenderTargetIdentifier currentTarget;

    //public FrostEffectPass(RenderPassEvent evt)
    //{
    //    renderPassEvent = evt;
    //    var shader = Shader.Find("Custom/ImageBlendEffect");
    //    if (shader == null)
    //    {
    //        Debug.LogError("Shader not found.");
    //        return;
    //    }
    //    frostEffectMaterial = CoreUtils.CreateEngineMaterial(shader);
    //}

    //public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    //{
    //    if (frostEffectMaterial == null)
    //    {
    //        Debug.LogError("Material not created.");
    //        return;
    //    }

    //    if (!renderingData.cameraData.postProcessEnabled) return;

    //    var stack = VolumeManager.instance.stack;
    //    frostEffect = stack.GetComponent<FrostEffect>();
    //    if (frostEffect == null) { return; }
    //    if (!frostEffect.IsActive()) { return; }

    //    var cmd = CommandBufferPool.Get(k_RenderTag);
    //    Render(cmd, ref renderingData);
    //    context.ExecuteCommandBuffer(cmd);
    //    CommandBufferPool.Release(cmd);
    //}

    //public void Setup(in RenderTargetIdentifier currentTarget)
    //{
    //    this.currentTarget = currentTarget;
    //}

    //void Render(CommandBuffer cmd, ref RenderingData renderingData)
    //{
    //    ref var cameraData = ref renderingData.cameraData;
    //    var source = currentTarget;
    //    int destination = TempTargetId;
    //    //if (!Application.isPlaying)
    //    //{
    //    //    frostEffectMaterial.SetTexture(BlendTexId, frostEffect.Frost);
    //    //    frostEffectMaterial.SetTexture(BumpMapId, frostEffect.FrostNormals);
    //    //    frostEffect.edgeSharpness.value = Mathf.Max(1, frostEffect.edgeSharpness.value);
    //    //}
    //    frostEffectMaterial.SetFloat(BlendAmountId, Mathf.Clamp01(Mathf.Clamp01(frostEffect.frostAmount.value) 
    //        * (frostEffect.maxFrost.value - frostEffect.minFrost.value) + frostEffect.minFrost.value));
    //    frostEffectMaterial.SetFloat(EdgeSharpnessId, frostEffect.edgeSharpness.value);
    //    frostEffectMaterial.SetFloat(SeeThroughnessId, frostEffect.seethroughness.value);
    //    frostEffectMaterial.SetFloat(DistortionId, frostEffect.distortion.value);

    //    int shaderPass = 0;
    //    cmd.SetGlobalTexture(MainTexId, source);
    //    //cmd.GetTemporaryRT(destination, w, h, 0, FilterMode.Point, RenderTextureFormat.Default);
    //    cmd.Blit(source, destination);
    //    cmd.Blit(destination, source, frostEffectMaterial, shaderPass);
    //}
//}
