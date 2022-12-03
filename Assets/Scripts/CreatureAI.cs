using BehaviourAI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Rotate : BehaviourAI.IAction
{
    [SerializeField]
    GameObject m_rotateTarget = default;
    [SerializeField]
    float m_rotateSpeed = 0.2f;
    [SerializeField]
    Animator m_anim = default;
    [SerializeField]
    string m_clipName = "AttackIdle";
    public GameObject target { set => m_target = value; }

    private GameObject m_target;
    public bool Reset { set => m_reset = value; }

    private bool m_reset;
    Vector3 relative;
    float angle;
    public bool End()
    {
        return m_reset;
    }

    public void Execute()
    {
        Vector3 relative = m_target.transform.InverseTransformPoint((m_rotateTarget == null ? CreatureAI.Target : m_rotateTarget).transform.position);
        angle = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg;
        if (Mathf.Abs(angle) > 5f)
        {
            m_target.transform.Rotate(0,angle > 0 ? m_rotateSpeed * m_anim.speed: -m_rotateSpeed * m_anim.speed,0);
            (m_anim == null?CreatureAI.Anim : m_anim).Play(m_clipName);
        }
        else
        {
            m_reset = true;
        }
    }
}

public class CheckAngle : BehaviourAI.IConditional
{
    [SerializeField]
    GameObject m_rotateTarget = default;
    [SerializeField]
    float m_angle = 45f;
    [SerializeField]
    bool m_isInside = true;
    public GameObject target { set => m_target = value; }

    private GameObject m_target;

    public bool Check()
    {
        Vector3 relative = m_target.transform.InverseTransformPoint((m_rotateTarget == null ? CreatureAI.Target : m_rotateTarget).transform.position);
        float angle = Mathf.Atan2(relative.x,relative.z) * Mathf.Rad2Deg;
        var rule = -m_angle < angle && angle < m_angle;
        return m_isInside ? rule : !rule;
    }
}
public class Move : BehaviourAI.IAction
{
    [SerializeField]
    float m_moveSpeed = 5f;

    [SerializeField]
    GameObject m_chaseTarget = default;

    [SerializeField]
    NavMeshAgent m_agent = default;

    [SerializeField]
    Animator m_anim = default;

    public GameObject target { set => m_target = value; }

    private GameObject m_target;
    public bool Reset { set => m_reset = value; }

    private bool m_reset;

    public bool End()
    {
        if (m_reset)
        {
            m_agent.SetDestination(m_target.transform.position);
        }
        return m_reset;
    }

    public void Execute()
    {
        if (m_agent.enabled == true)
        {
            m_agent.SetDestination((m_chaseTarget == null ? CreatureAI.Target : m_chaseTarget).transform.position);
            m_agent.speed = m_moveSpeed * m_anim.speed;
            m_anim.SetFloat("Speed", m_agent.speed);
            //Debug.Log("移動中");

            if (Vector3.Distance((m_chaseTarget == null ? CreatureAI.Target : m_chaseTarget).transform.position, m_target.transform.position) < m_agent.stoppingDistance)
            {
                m_agent.SetDestination(m_target.transform.position);
                m_anim.SetFloat("Speed", 0);
                //Debug.Log("移動停止");
                m_reset = true;
            }
        }
    }
}

public class Attack : IAction
{
    [SerializeField]
    string m_clipName = "Default";

    [SerializeField]
    Animator m_anim = default;

    [SerializeField]
    float m_coolTime = 2f;

    public GameObject target { set => m_target = value; }

    private GameObject m_target;
    public bool Reset { set => m_reset = value; }

    private bool m_reset;

    private float time = 0;

    private bool IsAnimate = false;

    public bool End()
    {
        if (m_reset)
        {
            time = 0;
            IsAnimate = false;
        }
        return m_reset;
    }

    public void Execute()
    {
        if (!IsAnimate)
        {
            m_anim.Play(m_clipName);
            IsAnimate = true;
        }
        
        //Debug.Log("攻撃中");
        if (time < m_coolTime)
        {
            time += Time.deltaTime;
        }

        else
        {
            m_reset = true;
        }
    }
}

public class CheckDistance : BehaviourAI.IConditional
{
    [SerializeField]
    float m_closeDistance = 10f;
    [SerializeField]
    float m_farDistance = 20f;

    [SerializeField]
    GameObject m_targetObject = default;
    public GameObject target { set => m_target = value; }

    private GameObject m_target;



    public bool Check()
    {
        var dis = Vector3.Distance(m_targetObject.transform.position, (m_target == null ? CreatureAI.Target : m_target).transform.position);
        bool InRange = m_closeDistance < dis && dis < m_farDistance;
        var log = InRange ? "範囲内" : "範囲外";
        //Debug.Log($"{log}{dis}");
        return InRange;
    }
}

public class CreatureAI : MonoBehaviour, BehaviourAI.IBehaviour
{
    [SerializeField]
    private BehaviourTreeManager m_treeManager = default;
    [SerializeField]
    private Animator m_anim = default;
    [SerializeField]
    private GameObject m_target;

    public static Animator Anim;

    public static GameObject Target;

    public static GameObject Player;
    private void Start()
    {
        Player = GameManager.Player.gameObject;
        Anim = m_anim;
        Target = m_target;
        m_treeManager.Repeater(this);
    }

    private void Update()
    {
        m_treeManager.Repeater(this);
    }
    public void Call(IAction Set)
    {
        Set.Execute();
    }

    public GameObject SetTarget()
    {
        return gameObject;
    }
}
