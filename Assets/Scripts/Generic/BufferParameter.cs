using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �l����莞�Ԏ��ԕς��p�����[�^�[�̏������`����
/// </summary>
public class BufferParameter
{
    float _value;

    float _rate = 1;

    public float Value => _value * _rate;

    public BufferParameter(float value) {_value = value; }

    /// <summary>
    /// �l����莞�ԕύX����(0�͉i��)
    /// </summary>
    /// <param name="rate">�ω�����</param>
    /// <param name="time">�ω�����(0�͉i��)</param>
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
