using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

/// <summary>
/// �U���^�C�v
/// </summary>
public enum ShakeType
{
    Damage,
    Roar,
    Hit,
}
/// <summary>
/// �R���g���[���[�̐U���Ɋւ��鏈���𐧌䂷��
/// </summary>
public class MotorShaker : MonoBehaviour
{
    public static MotorShaker Instance { get; set; }

    /// <summary>���ݎ��s���Ă���R���[�`��</summary>
    IEnumerator current;

    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// �R���g���[���[�̐U�����N����
    /// </summary>
    /// <param name="shakeType">�U���^�C�v</param>
    /// <param name="value">�U���̑傫��</param>
    public void Call(ShakeType shakeType,float value = 0)
    {
        if (Gamepad.current == null) return;
        current = null;
        switch (shakeType)
        {
            case ShakeType.Damage:
                current = value == 0 ? Damage(): Damage(value);
                break;
            case ShakeType.Roar:
                current = Roar();
                break;
            case ShakeType.Hit:
                current = value == 0 ? Hit() : Hit(value);
                break;
            default:
                break;
        }
        StartCoroutine(current);
    }

    /// <summary>
    /// player���_���[�W���󂯂��Ƃ��ɌĂ΂��
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    IEnumerator Damage(float value = 2)
    {
        Gamepad.current.SetMotorSpeeds(value, value);
        yield return new WaitForSeconds(0.5f);
        Gamepad.current.SetMotorSpeeds(0, 0);
    }

    /// <summary>
    /// player���_���[�W��^�����Ƃ��ɌĂ΂��
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    IEnumerator Hit(float value = 4)
    {
        Gamepad.current.SetMotorSpeeds(value * Random.Range(0.8f,1.2f), value * Random.Range(0.8f, 1.2f));
        yield return new WaitForSeconds(value * 0.05f);
        Gamepad.current.SetMotorSpeeds(0, 0);
    }

    /// <summary>
    /// �����X�^�[
    /// </summary>
    /// <returns></returns>
    IEnumerator Roar()
    {
        float speeds = 8;
        DOTween.To(() => speeds, (x) => speeds = x, 0, 2).SetEase(Ease.InOutBounce).OnUpdate(() =>
        {
            Gamepad.current.SetMotorSpeeds(speeds, speeds);
            Gamepad.current.SetMotorSpeeds(0, 0);
        }
        );
        yield return null;
    }
}


