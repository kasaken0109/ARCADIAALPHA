//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using DG.Tweening;

public enum EnemyState
{
    OutOfCombat,
    InBattle,
    Damage,
    Stun,
    Dead
}

public enum PosState
{
    FrontFarRange,
    FrontCloseRange,
    BackCloseRange,
    BackFarRange
}

[System.Serializable]
public class ActionSheet
{
    [SerializeReference, SubclassSelector]
    public ActionBase action = default;
    [Range(0, 100)]
    public int ActivationProbability = 10;
}
public class EnemyAI : MonoBehaviour
{
    [SerializeField]
    GameObject _target = default;
    [SerializeField]
    NavMeshAgent _navMeshAgent = default;
    [SerializeField]
    Animator _anim = default;
    [Header("====ここからアクション設定項目====")]
    [SerializeReference,SubclassSelector]
    [Header("前方遠くのアクション")]
    ActionSheet[] _FrontFarActionSheet = default;
    [SerializeReference, SubclassSelector]
    [Header("前方近くのアクション")]
    ActionSheet[] _FrontCloseActionSheet = default;
    [SerializeReference, SubclassSelector]
    [Header("後方近くのアクション")]
    ActionSheet[] _BackCloseActionSheet = default;
    [SerializeReference, SubclassSelector]
    [Header("後方遠くのアクション")]
    ActionSheet[] _BackFarActionSheet = default;
    EnemyState _enemyState = EnemyState.OutOfCombat;

    ActionBase action;
    Coroutine current = null;
    bool isEnable = true;
    Rigidbody _rb = default;
    Dictionary<System.Type, object> _componentInstance = new Dictionary<System.Type, object>();

    public void SetAIActive(bool isActive)
    {
        isEnable = isActive;
        _navMeshAgent.isStopped = !isActive;
        _rb.constraints = isActive 
            ? RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ 
            : RigidbodyConstraints.FreezeAll;

        if (!isActive)
        {
            if (current != null)StopCoroutine(current);
            action.Reset();
            return;
        }
        StartCoroutine(ExecuteAI());
    }
    void SetComponentInstance<T> (T instance) where T : Component
    {
        _componentInstance[typeof(T)] = instance;
    }

    public T GetComponentInstance<T>() where T : Component
    {
        return (T)_componentInstance[typeof(T)];
    }
    // Start is called before the first frame update
    void Awake()
    {
        TryGetComponent(out _rb);
        SetComponentInstance(_navMeshAgent);
        SetComponentInstance(_anim);
        SetComponentInstance(transform);
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        Invoke(nameof(InitExecuteAI), 2f);
    }

    void InitExecuteAI()
    {
        StartCoroutine(ExecuteAI());
    }

    IEnumerator ExecuteAI()
    {
        while (isEnable)
        {
            action = ActionProbabilitySet(SetSheet());
            current = StartCoroutine(action.Execute(_target.transform, this));
            yield return new WaitUntil(() => action.IsEndAction);
            action.Reset();
        }
    }

    ActionSheet[] SetSheet()
    {
        var isFar = Vector3.Distance(_target.transform.position, transform.position) >= 7f;
        Vector3 localFrontToWorld = transform.TransformDirection(Vector3.forward);
        Vector3 relative = _target.transform.position - transform.position;
        var angle = Vector3.Angle(localFrontToWorld, relative);
        var isFront = angle < 90f;
        var state = isFar ? (isFront ? PosState.FrontFarRange : PosState.BackFarRange) : (isFront ? PosState.FrontCloseRange : PosState.BackCloseRange);
        return state switch
        {
            PosState.FrontFarRange => _FrontFarActionSheet,
            PosState.FrontCloseRange => _FrontCloseActionSheet,
            PosState.BackFarRange => _BackFarActionSheet,
            PosState.BackCloseRange => _BackCloseActionSheet,
        };
    } 

    private ActionBase ActionProbabilitySet(ActionSheet[] actionSheets)
    {
        var percent = new int[100];
        int index = 0;
        for (int i = 0; i < actionSheets.Length; i++)
        {
            for (int k = 0; k < actionSheets[i].ActivationProbability; k++)
            {
                percent[index] = i;
                index++;
            }
        }
        int value = Random.Range(0,100);
        //Debug.Log(percent[value]);
        return actionSheets[percent[value]].action;
    }

    public void LookAt(float time = 1f)
    {
        transform.DOLookAt(_target.transform.position, time);
    }
}
