using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// バリアの体力、表示を管理する
/// </summary>
public class BarriaDisplay : MonoBehaviour,IDamage
{
    [SerializeField]
    [Tooltip("体力")]
    int _barriaHp = 2;
    [SerializeField]
    [Tooltip("バリアオブジェクトの表示位置")]
    Transform _chasePoint = default;
    [SerializeField]
    [Tooltip("耐久値減少時に発生するエフェクト")]
    GameObject _damage = default;
    [SerializeField]
    [Tooltip("耐久値減少時に変化するShaderオブジェクト")]
    GameObject _fragileEffect = default;
    [SerializeField]
    [Tooltip("破壊時に発生するエフェクト")]
    GameObject _crush = default;
    [SerializeField]
    [Tooltip("破壊時に起こるイベント")]
    UnityEvent _hitEvent = null;
    [SerializeField]
    float _rotateSpeed = 0.1f;

    /// <summary>現在の体力</summary>
    int hp = 0;

    /// <summary>
    /// バリアの体力減少時の処理
    /// ※バリアのIDamageインターフェースの実装
    /// </summary>
    /// <param name="damage">与えるダメージ</param>
    /// <param name="call">呼んだオブジェクト</param>
    public void AddDamage(int damage, ref GameObject call)
    {
        //ダメージを与えると耐久値と表示変更、耐久値が無くなったらバリア無効化
        if(hp > damage)
        {
            hp -= damage;
            SoundManager.Instance.PlayBarriaDamage();
            Instantiate(_damage, transform.position, transform.rotation);
            _fragileEffect.SetActive(hp == 1);
        }
        else
        {
            hp = 0;
            SoundManager.Instance.PlayBarriaFade();
            Instantiate(_crush,transform.position,transform.rotation);
            gameObject.SetActive(false); 
            _hitEvent?.Invoke();
        }
    }
    
    private void OnEnable()
    {
        //バリア有効時に耐久値をリセット
        hp = _barriaHp;
    }
    private void OnDisable()
    {
        //バリア破壊時にひび割れエフェクトを無効化
        _fragileEffect.SetActive(false);
    }

    void Update()
    {
        //バリアを付けたオブジェクトの位置を追従させる
        transform.position = _chasePoint.position + Vector3.up * 0.89f;
        transform.Rotate(Vector3.up * _rotateSpeed);
    }
}
