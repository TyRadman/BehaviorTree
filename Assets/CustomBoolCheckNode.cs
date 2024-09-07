using UnityEngine;

namespace BT.Nodes
{
	public class CustomBoolCheckNode : ConditionalCheckNode
	{
		[SerializeField] private bool _pass = true;

		protected override bool IsTrue()
		{
			return _pass;
		}
	}
}