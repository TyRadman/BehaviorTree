using UnityEngine;

namespace BT.Nodes
{
    using BT.Utilities;

    public abstract class ConditionalCheckNode : DecoratorNode
    {
        /// <summary>
        /// Defines how a condition node controls the execution flow of a behavior tree.
        /// Determines whether a running sequence is interrupted immediately when 
        /// the condition becomes false or allowed to complete before re-evaluation.
        /// </summary>
        [Tooltip("Defines how a condition node controls the execution flow of a behavior tree. Determines whether a running sequence is interrupted immediately when the condition becomes false or allowed to complete before re-evaluation.")]
        [SerializeField] private InterruptionMode _interruptionMode;
        [Tooltip("The state returned should the condition not be met.")]
        [SerializeField] private NodeState _returnState = NodeState.Failure;
        [SerializeField] private bool _invert = false;


        protected abstract bool IsTrue();

        protected override NodeState OnStart()
        {
            base.OnStart();

            if (_interruptionMode == InterruptionMode.Latched)
            {
                if (IsConditionBroken())
                {
                    return _returnState;
                }
            }

            return NodeState.Running;
        }


        protected sealed override NodeState OnUpdate()
        {
            if (_interruptionMode == InterruptionMode.Reactive)
            {
                if (IsConditionBroken())
                {
                    Child.Interrupt();
                    return _returnState;
                }

                var childState = Child.Update();

                return childState;
            }
            else
            {
                var childState = Child.Update();

                return childState;
            }
        }

        private bool IsConditionBroken()
        {
            return !IsTrue() ^ _invert;
        }
    }
}
