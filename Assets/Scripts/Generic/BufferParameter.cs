using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 値が一定時間時間変わるパラメーターの処理を定義する
/// </summary>
public class BufferParameter
{
    float _value;

    float _rate = 1;

    public float Value => _value * _rate;

    public BufferParameter(float value) {_value = value; }

    /// <summary>
    /// 値を一定時間変更する(0は永続)
    /// </summary>
    /// <param name="rate">変化割合</param>
    /// <param name="time">変化時間(0は永続)</param>
    /// <returns></returns>
    public IEnumerator ChangeValue(float rate, float time)
    {
        _rate = rate;
        if (time != 0)
        {
            yield return new WaitForSeconds(time);
            _rate = 1;
        }
        else
        {
            yield return null;
        }
    }

}
