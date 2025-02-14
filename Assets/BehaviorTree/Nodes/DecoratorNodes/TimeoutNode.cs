using UnityEngine;

namespace BT.Nodes
{
    using Utilities;

    [NodePath(DECORATOR_NODE_PATH + "Timeout Node")]
    public class TimeoutNode : DecoratorNode
    {
        [SerializeField] private TimeMode _timeMode = TimeMode.DeltaTime;
        [SerializeField] private InterruptionMode _interruptionMode = InterruptionMode.Reactive;
        [SerializeField] private float _timeoutDuration = 1.0f;

        private float _timeElapsed;

        public override void OnAwake()
        {
            base.OnAwake();

            if (_timeoutDuration < 0f)
            {
                _timeoutDuration = 0f;
                Debug.LogError($"Timeout duration at node {ViewDetails.Name} cannot be negative. Setting it to 0.");
            }
        }

        protected override void OnStart()
        {
            base.OnStart();

            _timeElapsed = 0.0f;
        }

        protected override NodeState OnUpdate()
        {
            NodeState childState = Child.Update();

            if (_timeElapsed >= _timeoutDuration)
            {
                return GetStateOnTimeout(childState);
            }

            UpdateTimer();

            return childState;
        }

        private NodeState GetStateOnTimeout(NodeState childState)
        {
            if (_interruptionMode == InterruptionMode.Reactive)
            {
                return NodeState.Success;
            }
            else
            {
                return childState;
            }
        }

        private void UpdateTimer()
        {
            if(_timeMode == TimeMode.DeltaTime)
            {
                UpdateTimerWithDeltaTime();
            }
            else
            {
                UpdateTimerWithUnscaledDeltaTime();
            }
        }

        private void UpdateTimerWithUnscaledDeltaTime()
        {
            _timeElapsed += Time.unscaledDeltaTime;
        }

        private void UpdateTimerWithDeltaTime()
        {
            _timeElapsed += Time.deltaTime;
        }
    }
}