using UnityEngine;

namespace BT.Nodes
{
	using Utilities;

	[NodePath(DECORATOR_NODE_PATH + "Loop Node")]
	public class LoopNode : DecoratorNode
	{
		[SerializeField] private LoopMode _loopMode = LoopMode.Finite;
		[SerializeField] private int _loopsToComplete = 3;

        private int _loopsCompleted;

		protected override void OnStart()
		{
			_loopsToComplete = Mathf.Max(1, _loopsToComplete);

			_loopsCompleted = 0;
		}

		protected override NodeState OnUpdate()
		{
			NodeState childState = Child.Update();

			if (_loopMode == LoopMode.Finite)
			{
				if (childState is NodeState.Failure or NodeState.Success)
				{
					_loopsCompleted++;
				}

				if (_loopsCompleted >= _loopsToComplete)
				{
					return NodeState.Success;
				}

				return NodeState.Running;
			}
			else
			{
				return NodeState.Running;
			}
		}

		protected override void OnExit()
		{

		}
	}
}