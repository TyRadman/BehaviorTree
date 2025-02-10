using UnityEngine;

namespace BT.Nodes
{
	public class ActionCheck : ActionNode
	{
		[SerializeField] private bool _pass = true;
		public BlackboardKey Key;

		protected override void OnStart()
		{
			// start logic
		}

		protected override NodeState OnUpdate()
		{
			return _pass? NodeState.Success : NodeState.Failure;
		}

		protected override void OnExit()
		{
			// stop logic
		}
	}
}