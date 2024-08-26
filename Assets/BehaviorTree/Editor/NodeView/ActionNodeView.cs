using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace BT.NodesView
{
    public class ActionNodeView : NodeView
    {
        [HideInInspector]
        public override string StyleClassName { get; set; } = "action";

        protected override void CreateInputPort()
        {
            CreatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single);
        }

        protected override void CreateOutputPort()
        {

        }
    }
}
