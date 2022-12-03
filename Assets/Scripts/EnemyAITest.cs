using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum TargetType
{
    Distance,
    Hate,
    HP
}
public class EnemyAITest : MonoBehaviour
{
    [SerializeField]
    string _defaultTargetTag = "Player";

    Animator _anim;

    ITarget currentTarget;
    // Start is called before the first frame update
    void Start()
    {
        TryGetComponent(out _anim);
        currentTarget = SetTarget<PlayerManager>(_defaultTargetTag);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    T SetTarget<T>(string targetTag) where T : Object,ITarget{
        List<T>targets = FindObjectsOfType<T>().ToList();
        var t = targets.Where(c => c.GetTargetTag() == targetTag);
        return SetTargetSingle<T>(targets);
    }

    T SetTargetSingle<T>(List<T> targetList,TargetType targetType = TargetType.Distance) where T : UnityEngine.Object, ITarget
    {
        T target = targetList.First();
        switch (targetType)
        {
            case TargetType.Distance:
                target = targetList.OrderBy(c => Vector3.Distance(c.GetTargetPos(), transform.position)).First();
                break;
            case TargetType.Hate:
                break;
            case TargetType.HP:
                break;
            default:
                break;
        }
        return target;
    }
}
