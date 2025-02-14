using UnityEngine;

namespace BT.Nodes
{
	[NodePath("Custom/Node/Path/New Node")]
	public class NewNode : ActionNode
	{
		protected override void OnStart()
		{
			// start logic
		}

		protected override NodeState OnUpdate()
		{
			return NodeState.Success;
		}

		protected override void OnExit()
		{
		}
	}
}