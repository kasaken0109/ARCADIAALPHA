using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// �o���A�̗̑́A�\�����Ǘ�����
/// </summary>
public class BarriaDisplay : MonoBehaviour,IDamage
{
    [SerializeField]
    [Tooltip("�̗�")]
    int _barriaHp = 2;
    [SerializeField]
    [Tooltip("�o���A�I�u�W�F�N�g�̕\���ʒu")]
    Transform _chasePoint = default;
    [SerializeField]
    [Tooltip("�ϋv�l�������ɔ�������G�t�F�N�g")]
    GameObject _damage = default;
    [SerializeField]
    [Tooltip("�ϋv�l�������ɕω�����Shader�I�u�W�F�N�g")]
    GameObject _fragileEffect = default;
    [SerializeField]
    [Tooltip("�j�󎞂ɔ�������G�t�F�N�g")]
    GameObject _crush = default;
    [SerializeField]
    [Tooltip("�j�󎞂ɋN����C�x���g")]
    UnityEvent _hitEvent = null;
    [SerializeField]
    float _rotateSpeed = 0.1f;

    /// <summary>���݂̗̑�</summary>
    int hp = 0;

    /// <summary>
    /// �o���A�̗̑͌������̏���
    /// ���o���A��IDamage�C���^�[�t�F�[�X�̎���
    /// </summary>
    /// <param name="damage">�^����_���[�W</param>
    /// <param name="call">�Ă񂾃I�u�W�F�N�g</param>
    public void AddDamage(int damage, ref GameObject call)
    {
        //�_���[�W��^����Ƒϋv�l�ƕ\���ύX�A�ϋv�l�������Ȃ�����o���A������
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
        //�o���A�L�����ɑϋv�l�����Z�b�g
        hp = _barriaHp;
    }
    private void OnDisable()
    {
        //�o���A�j�󎞂ɂЂъ���G�t�F�N�g�𖳌���
        _fragileEffect.SetActive(false);
    }

    void Update()
    {
        //�o���A��t�����I�u�W�F�N�g�̈ʒu��Ǐ]������
        transform.position = _chasePoint.position + Vector3.up * 0.89f;
        transform.Rotate(Vector3.up * _rotateSpeed);
    }
}
