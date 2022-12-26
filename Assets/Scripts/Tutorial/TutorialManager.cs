using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

/// <summary>
/// チュートリアルの管理を行う
/// </summary>
public class TutorialManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("チュートリアル開始時に選択状態にするボタン")]
    Button _focusButtonOnTutorialStart = default;
    [SerializeField]
    [Tooltip("チュートリアル開始後に選択状態にするオブジェクト")]
    GameObject _prevFocus = default;
    [SerializeField]
    [Tooltip("チュートリアルスキップ時に実行されるイベント")]
    UnityEvent _OnTutorialSkip = default;

    /// <summary>選択状態になっているボタン</summary>
    GameObject _prev = default;
    /// <summary>チュートリアルがスキップされたか</summary>
    bool isTutorialSkipped = false;
    private void Awake()
    {
        //サービスロケーターへインスタンスを登録
        ServiceLocator.SetInstance(this);
        ButtonSelect();
    }

    private void OnDestroy()
    {
        //サービスロケーターのインスタンスを削除
        ServiceLocator.RemoveInstance<TutorialManager>();
    }

    /// <summary>
    /// チュートリアルシーン開始時にボタンを選択状態にする
    /// </summary>
    void ButtonSelect()
    {
        //イベントシステムに選択されていたオブジェクトを記憶
        _prev = EventSystem.current.currentSelectedGameObject ?? _prevFocus; 
        EventSystem.current.SetSelectedGameObject(_focusButtonOnTutorialStart.gameObject);
    }

    public void ResetFocusSelectable()
    {
        //選択状態のオブジェクトが無い場合ボタンUI有効時に選ばれるオブジェクトを設定
        if (!_prev) _prev = EventSystem.current.currentSelectedGameObject;
        //選択状態をリセット
        EventSystem.current.SetSelectedGameObject(_prev);
    }

    /// <summary>
    /// チュートリアルスキップ時に実行する
    /// </summary>
    public void OnSkipTutorial()
    {
        //イベントを実行
        _OnTutorialSkip?.Invoke();
        isTutorialSkipped = true;
    }
}
