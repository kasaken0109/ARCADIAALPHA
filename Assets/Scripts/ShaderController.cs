using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// タイトルのShaderの処理を行う
/// </summary>
public class ShaderController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("設定するマテリアル")]
    Material _set = default;
    /// <summary>現在の時間</summary>
    float time = 0;
    /// <summary>現在のシーンに移るまでの時間</summary>
    float timeUntilSceneStart;
    /// <summary>マテリアルを設定したか</summary>
    bool IsSetMaterial = false;
    /// <summary>セット対象</summary>
    Image render;
    

    // Start is called before the first frame update
    void Start()
    {
        time = 0;
        TryGetComponent(out render);
        render.material = null;
        timeUntilSceneStart = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsSetMaterial) time += Time.deltaTime;
    }

    /// <summary>
    /// 消えるマテリアルを設定する
    /// </summary>
    public void SetMaterialProparty()
    {
        IsSetMaterial = false;
        if (render)
        {
            render.material = _set;
            //シーン開始からの時間をセット
            render.material.SetFloat("_TimeScale", timeUntilSceneStart + time);
        }
        else
        {
            Debug.LogError("mee");
        }
    }
}
