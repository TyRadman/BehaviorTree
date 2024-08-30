using UnityEngine;

namespace BT.Nodes
{
	public class BoolAction : ActionNode
	{
		protected override void OnStart()
		{
			// start logic
		}

		protected override NodeState OnUpdate()
		{
			// update logic
			return NodeState.Success;
		}

		protected override void OnStop()
		{
			// stop logic
		}
	}
}