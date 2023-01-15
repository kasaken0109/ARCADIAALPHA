using System.Collections;
using System.Text.RegularExpressions;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UniRx;

public class TextSender : MonoBehaviour
{
    [SerializeField]
    float _sendTime = 1f;
    Text _display = default;

    public bool IsCompleted => _isCompleted;
    bool _isCompleted = false;
    private void Awake()
    {
        TryGetComponent(out _display);
        //SendText("Ç±ÇÍÇÕ_<color=green>ÉJÉCÉäÉÖÅ[</color>_Ç≈Ç∑");
        Observable.Timer(TimeSpan.FromSeconds(0.5f)).Subscribe(_ => SkipText());
    }

    bool isStart = true;
    string skipWord;
    public void SendText(string displayText)
    {
        IEnumerator DelayShowText(string text)
        {
            _isCompleted = false;
            _display.text = "";
            for (int i = 0; i < text.Length; i++)
            {
                if (isSkipped)
                {
                    _display.text = text.Replace("_","");
                    isSkipped = false;
                    _isCompleted = true;
                    yield break;
                }
                if (text[i] == '_' && isStart)
                {
                    isStart = false;
                    continue;
                }
                if (!isStart)
                {
                    if (text[i] == '_')
                    {
                        _display.text += skipWord;
                        yield return new WaitForSeconds(_sendTime);
                        skipWord = "";
                        isStart = true;
                        continue;
                    }
                    skipWord += text[i];
                    continue;
                }
                _display.text += text[i];
                yield return new WaitForSeconds(_sendTime);
            }
            _isCompleted = true;
        }
        StartCoroutine(DelayShowText(displayText));
    }

    bool isSkipped = false;
    public void SkipText()
    {
        isSkipped = true;
    }
}
