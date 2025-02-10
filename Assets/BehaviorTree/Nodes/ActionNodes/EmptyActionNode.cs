using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT.Nodes
{
#if UNITY_EDITOR
    public class EmptyActionNode : ActionNode
    {
        [SerializeField] private NodeState _nodeState = NodeState.Success;

        protected override void OnStart()
        {
        }

        protected override void OnStop()
        {

        }

        protected override NodeState OnUpdate()
        {
            return _nodeState;
        }
    }
#endif
}