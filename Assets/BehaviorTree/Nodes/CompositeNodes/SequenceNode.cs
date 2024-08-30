using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT.Nodes
{
    public class SequenceNode : CompositeNode
    {
        private int _currentChild;

        protected override void OnStart()
        {
            _currentChild = 0;
            Debug.Log("Started sequence");
        }

        protected override void OnStop()
        {

        }

        protected override NodeState OnUpdate()
        {
            BaseNode child = Children[_currentChild];
            NodeState state = child.Update();

            switch (state)
            {
                case NodeState.Running:
                    {
                        return NodeState.Running;
                    }
                case NodeState.Failure:
                    {
                        Debug.Log($"Failure {_currentChild}");
                        return NodeState.Failure;
                    }
                case NodeState.Success:
                    {
                        Debug.Log($"Success {_currentChild}");
                        break;
                    }
            }

            _currentChild++;

            if (_currentChild == Children.Count)
            {
                return NodeState.Success;
            }
            else
            {
                return NodeState.Running;
            }
        }
    }
}
