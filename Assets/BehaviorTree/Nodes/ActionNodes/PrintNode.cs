using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT.Nodes
{
    public class PrintNode : ActionNode
    {
        [TextArea(2, 4)] 
        [SerializeField] private string _message;
        //public override string SearchDirectory { get; set; } = "Defaults/Action";

        protected override void OnStart()
        {
            Debug.Log($"{_message}");
        }

        protected override void OnStop()
        {
            //Debug.Log($"OnStop: {_message}");
        }

        protected override NodeState OnUpdate()
        {
            //Debug.Log($"OnUpdate: {_message}");
            return NodeState.Success;
        }
    }
}
