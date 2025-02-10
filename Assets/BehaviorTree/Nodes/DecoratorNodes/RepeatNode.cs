using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT.Nodes
{
#if UNITY_EDITOR
    public class RepeatNode : DecoratorNode
    {
        protected override void OnStart()
        {

        }

        protected override void OnStop()
        {

        }

        protected override NodeState OnUpdate()
        {
            Child.Update();

            return NodeState.Running;
        }
    }
#endif
}
