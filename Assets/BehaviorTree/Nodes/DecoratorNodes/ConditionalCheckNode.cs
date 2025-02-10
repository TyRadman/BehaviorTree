using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT.Nodes
{
    using BT.Utilities;

#if UNITY_EDITOR
    public abstract class ConditionalCheckNode : DecoratorNode
    {
        /// <summary>
        /// Defines how a condition node controls the execution flow of a behavior tree.
        /// Determines whether a running sequence is interrupted immediately when 
        /// the condition becomes false or allowed to complete before re-evaluation.
        /// </summary>
        [Tooltip("Defines how a condition node controls the execution flow of a behavior tree. Determines whether a running sequence is interrupted immediately when the condition becomes false or allowed to complete before re-evaluation.")]
        [SerializeField] private InterruptionMode _interruptionMode;
        protected abstract bool IsTrue();

        protected override NodeState OnUpdate()
        {
            if (_interruptionMode == InterruptionMode.Reactive)
            {
                if (!IsTrue())
                {
                    return NodeState.Failure;
                }

                return Child.Update();
            }
            else if(_interruptionMode == InterruptionMode.Latched)
            {
                if(Child.Update() != NodeState.Running)
                {
                    return IsTrue() ? NodeState.Success : NodeState.Failure;
                }
                else
                {
                    return NodeState.Running;
                }
            }
            else
            {
                return NodeState.Running;
            }
        }
    }
#endif
}
