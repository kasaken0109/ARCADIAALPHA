using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �^�C�g����Shader�̏������s��
/// </summary>
public class ShaderController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("�ݒ肷��}�e���A��")]
    Material _set = default;
    /// <summary>���݂̎���</summary>
    float time = 0;
    /// <summary>���݂̃V�[���Ɉڂ�܂ł̎���</summary>
    float timeUntilSceneStart;
    /// <summary>�}�e���A����ݒ肵����</summary>
    bool IsSetMaterial = false;
    /// <summary>�Z�b�g�Ώ�</summary>
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
    /// ������}�e���A����ݒ肷��
    /// </summary>
    public void SetMaterialProparty()
    {
        IsSetMaterial = false;
        if (render)
        {
            render.material = _set;
            //�V�[���J�n����̎��Ԃ��Z�b�g
            render.material.SetFloat("_TimeScale", timeUntilSceneStart + time);
        }
        else
        {
            Debug.LogError("mee");
        }
    }
}
