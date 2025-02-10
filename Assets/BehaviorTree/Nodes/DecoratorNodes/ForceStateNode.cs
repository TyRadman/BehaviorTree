using UnityEngine;

namespace BT.Nodes
{
#if UNITY_EDITOR
	public class ForceStateNode : DecoratorNode
	{
		[SerializeField] private NodeState _returnedState;

		protected override void OnStart()
		{
		}

		protected override NodeState OnUpdate()
		{
			NodeState state = Child.Update();

			if (state == NodeState.Running)
			{
				return state;
			}

			return _returnedState;
		}

		protected override void OnExit()
		{
			// end logic
		}
	}
#endif
}