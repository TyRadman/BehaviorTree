using UnityEngine;

namespace BT.Nodes
{
	public class BoolCheckLoopNode : ConditionalLoopNode
	{
		[SerializeField] private bool _pass = true;

		protected override bool IsTrue()
		{
			// condition. If true, the loop will go on, otherwise, it will stop.
			return _pass;
		}
	}
}