using UnityEngine;

namespace BT.Nodes
{
	public class ActionCheck : ActionNode
	{
		[SerializeField] private bool _pass = true;

		protected override void OnStart()
		{
			// start logic
		}

		protected override NodeState OnUpdate()
		{
			// update logic
			return NodeState.Failure;
			return _pass? NodeState.Success : NodeState.Failure;
		}

		protected override void OnStop()
		{
			// stop logic
		}
	}
}