using UnityEngine;

namespace BT.Nodes
{
	public class BoolCheckNode : ConditionalCheckNode
	{
		[SerializeField] private bool _pass = true;

		protected override bool IsTrue()
		{
			return _pass;
		}
	}
}