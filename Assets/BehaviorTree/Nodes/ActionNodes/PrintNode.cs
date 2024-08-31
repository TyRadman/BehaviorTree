using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT.Nodes
{
    public class PrintNode : ActionNode
    {
        [TextArea(2, 4)] 
        [SerializeField] private string _message;

        protected override void OnStart()
        {
            Debug.Log($"{_message}");
        }

        protected override void OnStop()
        {

        }

        protected override NodeState OnUpdate()
        {
            return NodeState.Success;
        }
    }
}
