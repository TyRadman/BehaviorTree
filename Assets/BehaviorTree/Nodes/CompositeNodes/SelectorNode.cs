using UnityEngine;

namespace BT.Nodes
{
    [NodePath(COMPOSITE_NODE_PATH + "Selector Node")]
    public class SelectorNode : CompositeNode
    {
        private int _currentChild;
        
        protected override void OnStart()
		{
			_currentChild = 0;
		}

		protected override NodeState OnUpdate()
        {
            BaseNode child = Children[_currentChild];

            switch (child.Update())
            {
                case NodeState.Running:
                    {
                        return NodeState.Running;
                    }
                case NodeState.Failure:
                    {
                        break;
                    }
                case NodeState.Success:
                    {
                        return NodeState.Success;
                    }
            }

            _currentChild++;

            if (_currentChild == Children.Count)
            {
                return NodeState.Failure;
            }
            else
            {
                return NodeState.Running;
            }
        }

		protected override void OnExit()
		{
			// end logic
		}
	}
}