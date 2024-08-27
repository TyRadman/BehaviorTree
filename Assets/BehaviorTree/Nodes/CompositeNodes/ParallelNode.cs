using System.Collections.Generic;
using UnityEngine;

namespace BT.Nodes
{
	public class ParallelNode : CompositeNode
	{
        protected override void OnStart()
		{

		}

		protected override NodeState OnUpdate()
		{
			bool allSuccess = true;

            for (int i = 0; i < Children.Count; i++)
            {
				NodeState state = Children[i].Update();

				if(state == NodeState.Failure)
                {
					return NodeState.Failure;
                }

				if(state == NodeState.Running)
                {
					allSuccess = false;
				}
            }

			return allSuccess ? NodeState.Success : NodeState.Running;
		}

		protected override void OnStop()
		{

		}
	}
}