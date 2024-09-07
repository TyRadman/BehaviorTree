using UnityEngine;

namespace BT.Nodes
{
	public class CheckLoop : ConditionalLoopNode
	{
		protected override bool IsTrue()
		{
			// condition. If true, the loop will go on, otherwise, it will stop.
			return true;
		}
	}
}