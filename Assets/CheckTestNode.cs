using UnityEngine;

namespace BT.Nodes
{
	public class CheckTestNode : ConditionalCheckNode
	{
		protected override bool IsTrue()
		{
			// condition
			return true;
		}
	}
}