using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BehaviourAI
{
    public interface IBehaviour
    {
        /// <summary>
        /// 呼び出し元を設定する
        /// </summary>
        /// <returns></returns>
        GameObject SetTarget();
        /// <summary>
        /// BehaviorTreeの呼び出し
        /// </summary>
        /// <param name="Set">セットするアクション</param>
        void Call(IAction Set);
    }
    public interface IConditional
    {
        GameObject target { set; }

        /// <summary>
        /// 遷移条件にあっているかどうか
        /// </summary>
        /// <returns></returns>
        bool Check();
    }

    public interface IAction
    {
        GameObject target { set; }

        /// <summary>
        /// 実行時に呼ばれる関数
        /// </summary>
        void Execute();

        /// <summary>
        /// 終了時に呼ばれる関数
        /// </summary>
        /// <returns>終了したかどうか</returns>
        bool End();

        bool Reset { set; }
    }

    public class BehaviourTreeManager : MonoBehaviour
    {
        enum State
        {
            Run,
            Set,
            None,
        }

        State state = State.None;
        [SerializeField]
        List<Selector> selectors = new List<Selector>();

        [System.Serializable]
        public class Selector
        {
            [SerializeField]
            public List<Sequence> Sequences = new List<Sequence>();
            
            [System.Serializable]
            public class Sequence
            {
                [SerializeReference, SubclassSelector]
                public IConditional Conditional;
                [SerializeReference, SubclassSelector]
                public IAction Action;
                [SerializeReference, SubclassSelector]
                public IAction FailAction;
            }
        }

        SelectorNode m_selectorN;
        SequenceNode m_sequenceN;

        public void Repeater<T>(T get) where T:IBehaviour
        {
            GameObject target = get.SetTarget();
            switch (state)
            {
                case State.Run:
                    m_sequenceN.Set(selectors[m_selectorN.GetID].Sequences[m_sequenceN.SequenceID], get, ref state, target);
                    break;
                case State.Set:
                    state = State.None;
                    break;
                case State.None:
                    m_selectorN = new SelectorNode();
                    m_sequenceN = new SequenceNode();
                    state = State.Set;
                    m_selectorN.Set(selectors,m_sequenceN,ref state,target);
                    break;
            }
        }

        class SelectorNode
        {
            public int GetID => m_id;

            private int m_id = 0;

            public void Set(List<Selector> selector,SequenceNode sequenceN,ref State state,GameObject target)
            {
                ConditionalNode conditionalN = new ConditionalNode();
                conditionalN.SetTarget = target;
                if (selector.Count <= 0)
                {
                    Debug.LogError("データが登録されていません");
                    return;
                }
                else if (selector.Count == 1)
                {
                    m_id = 0;
                    conditionalN.Set(selector[0], sequenceN, ref state);
                }
                else
                {
                    int r = Random.Range(0,selector.Count);
                    m_id = r;
                    conditionalN.Set(selector[m_id], sequenceN, ref state);
                    //m_id = m_id == selector.Count - 1 ? 0 : m_id++; 
                }
            }
        }

        class ConditionalNode
        {
            public GameObject SetTarget { private get; set; }

            public void Set(Selector selector,SequenceNode sequenceN,ref State state)
            {
                for (int i = 0; i < selector.Sequences.Count; i++)
                {
                    IConditional conditional = selector.Sequences[i].Conditional;
                    conditional.target = SetTarget;
                    if (conditional.Check())
                    {
                        state = State.Run;
                        sequenceN.SequenceID = i;
                        return;
                    }
                }
                state = State.None;
            }


        }

        class SequenceNode
        {
            public int SequenceID { get; set; }

            public void Set(Selector.Sequence sequence,IBehaviour behaviour,ref State state,GameObject target)
            {
                ActionNode actionN = new ActionNode();
                actionN.SetTarget = target;
                if (sequence.Conditional.Check()) actionN.Set(sequence.Action, behaviour, ref state);
                else
                {
                    if(sequence.FailAction == null)
                    {
                        sequence.Action.Reset = false;
                        state = State.None;
                    }
                    else actionN.Set(sequence.FailAction, behaviour, ref state);
                    //sequence.Action.Reset = false;
                    //state = State.None;
                }
            }
        }

        class ActionNode
        {
            public GameObject SetTarget { get; set; }

            public void Set(IAction action,IBehaviour behaviour, ref State state)
            {
                action.target = SetTarget;
                if (action.End())
                {
                    state = State.None;
                    action.Reset = false;
                }
                else behaviour.Call(action);
            }
        }
    }
}
