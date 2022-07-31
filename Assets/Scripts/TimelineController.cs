using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

/// <summary>
/// �^�C�����C���Đ����̏������s��
/// </summary>
public class TimelineController : MonoBehaviour
{
    /// <summary>text��List</summary>
    List<Text> _textList;
    /// <summary>Image��List</summary>
    List<Image> _imageList;
    // Start is called before the first frame update
    void Start()
    {
        //�e�L�X�g���摜������������A�\�����Ă���UI�Ƀ��X�g���i��
        _textList = FindObjectsOfType<Text>().Where(x => x.gameObject.activeInHierarchy == true).ToList();
        _imageList = FindObjectsOfType<Image>().Where(x => x.gameObject.activeInHierarchy == true).ToList();
        SetActiveUI(false);
    }

    /// <summary>
    /// UI�O���[�v��SetActive����
    /// </summary>
    /// <param name="isActive">�\�����邩</param>
    public void SetActiveUI(bool isActive)
    {
        _textList.ForEach(x => x.enabled = isActive);
        _imageList.ForEach(x => x.enabled = isActive);
    }

    private void OnDisable()
    {
        SetActiveUI(true);
    }
}
