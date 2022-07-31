using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

/// <summary>
/// タイムライン再生時の処理を行う
/// </summary>
public class TimelineController : MonoBehaviour
{
    /// <summary>textのList</summary>
    List<Text> _textList;
    /// <summary>ImageのList</summary>
    List<Image> _imageList;
    // Start is called before the first frame update
    void Start()
    {
        //テキストを画像を検索した後、表示しているUIにリストを絞る
        _textList = FindObjectsOfType<Text>().Where(x => x.gameObject.activeInHierarchy == true).ToList();
        _imageList = FindObjectsOfType<Image>().Where(x => x.gameObject.activeInHierarchy == true).ToList();
        SetActiveUI(false);
    }

    /// <summary>
    /// UIグループをSetActiveする
    /// </summary>
    /// <param name="isActive">表示するか</param>
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
