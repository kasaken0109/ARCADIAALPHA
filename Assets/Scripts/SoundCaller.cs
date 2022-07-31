using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Audio;

/// <summary>
/// �V�[����SoundManager���Ăяo���p�̃N���X
/// </summary>
public class SoundCaller : MonoBehaviour
{
    [SerializeField]
    [Tooltip("�V�[���ɐݒ肷��BGM")]
    AudioClip _bgm;
    [SerializeField]
    [Tooltip("�{�^���ɓo�^����SE��Defaut���ǂ���")]
    bool _canSetSE = true;

    void Start()
    {
        //BGM�Đ�
        if(_bgm)SoundManager.Instance.PlayBGM(_bgm);
        if (_canSetSE)
        {
            //�{�^���������A���̍Đ�������ǉ�����
            var button = FindObjectsOfType<Button>();
            button.ToList().ForEach(x => x.onClick.AddListener(() => SoundManager.Instance.PlayClick()));
        }
    }

    /// <summary>
    /// OnClick�p�̃N���b�N���Đ�����
    /// </summary>
    /// <param name="clip">�N���b�N��</param>
    public void PlayButtonSE(AudioClip clip)
    {
        SoundManager.Instance.PlayClick(clip);
    }
}
