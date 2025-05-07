using UnityEngine;

namespace BT.Nodes
{
    [NodePath(COMPOSITE_NODE_PATH + "Parallel Node")]
    public class ParallelNode : CompositeNode
	{
		public enum ParallelType
		{
			OneTriggeredChild = 0, AllTriggeredChildren = 1, NumberOfTriggeredChildren = 2
		}

		[SerializeField] private ParallelType _failureCondition;
		[SerializeField] private NodeState _returnState;
		[SerializeField] private NodeState _childTriggerState;
		[Tooltip("Takes effect only if the Failure Condition is set to NumberOfTriggeredChildren.")]
		[SerializeField] public int ConditionalFailedChildrenCount = 1;

		protected override NodeState OnStart()
        {
            return NodeState.Running;
        }

		protected override NodeState OnUpdate()
		{
			switch (_failureCondition)
			{
				case ParallelType.OneTriggeredChild:
					return PerformOneChildFailLogic();
				case ParallelType.AllTriggeredChildren:
					return PerformAllChildrenFailLogic();
				case ParallelType.NumberOfTriggeredChildren:
					return PerformNumberOfChildrenFailLogic();
				default:
					return NodeState.Running;
			}
        }

        private NodeState PerformOneChildFailLogic()
        {
            for (int i = 0; i < Children.Count; i++)
            {
                NodeState state = Children[i].State;

                if (state == _childTriggerState)
                {
					for (int j = 0; j < Children.Count; j++)
					{
						Children[j].Interrupt();
					}

                    return _returnState;
                }
            }

            for (int i = 0; i < Children.Count; i++)
            {
                Children[i].Update();
            }

            return NodeState.Running;
        }

        private NodeState PerformAllChildrenFailLogic()
        {
            bool allFailed = true;

            for (int i = 0; i < Children.Count; i++)
            {
                NodeState state = Children[i].Update();

                if (state != _childTriggerState)
                {
                    allFailed = false;
                }
            }

            return allFailed ? _returnState : NodeState.Running;
        }

        private NodeState PerformNumberOfChildrenFailLogic()
		{
			int failedChildrenCount = 0;

			for (int i = 0; i < Children.Count; i++)
			{
				NodeState state = Children[i].Update();

                if (state == _childTriggerState)
				{
					failedChildrenCount++;
				}

				if (failedChildrenCount >= ConditionalFailedChildrenCount)
				{
					return _returnState;
				}
			}

			return NodeState.Running;
		}

		protected override void OnExit()
		{

		}

        public override void Interrupt()
        {
            base.Interrupt();

			for (int i = 0; i < Children.Count; i++)
			{
				Children[i].Interrupt();
			}
        }

        public override bool Abort()
        {
			if (base.Abort())
            {
                for (int i = 0; i < Children.Count; i++)
                {
                    Children[i].Abort();
                }

                return true;
			}

			return false;
        }
    }
}