using UnityEngine;

namespace BT.Nodes
{
	public class NewNode : ActionNode
	{
		public BlackboardKey _customKey;

		protected override void OnStart()
		{
			// start logic
		}

		protected override NodeState OnUpdate()
		{
			// update logic
			return NodeState.Success;
		}

		protected override void OnExit()
		{
			// stop logic
		}
	}
}