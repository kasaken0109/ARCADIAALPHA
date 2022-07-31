using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

/// <summary>
/// 振動タイプ
/// </summary>
public enum ShakeType
{
    Damage,
    Roar,
    Hit,
}
/// <summary>
/// コントローラーの振動に関する処理を制御する
/// </summary>
public class MotorShaker : MonoBehaviour
{
    public static MotorShaker Instance { get; set; }

    /// <summary>現在実行しているコルーチン</summary>
    IEnumerator current;

    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// コントローラーの振動を起こす
    /// </summary>
    /// <param name="shakeType">振動タイプ</param>
    /// <param name="value">振動の大きさ</param>
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
    /// playerがダメージを受けたときに呼ばれる
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
    /// playerがダメージを与えたときに呼ばれる
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
    /// モンスター
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


