using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT.Nodes
{
    public abstract class ActionNode : BaseNode
    {
        public override List<BaseNode> GetChildren()
        {
            base.GetChildren();
            return new List<BaseNode>();
        }

        public override bool HasChildren()
        {
            return false;
        }

        public override void StopNode()
        {
            State = NodeState.Success;
        }
    }
}
