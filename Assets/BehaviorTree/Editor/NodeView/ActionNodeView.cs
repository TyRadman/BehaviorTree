using BT.Nodes;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace BT.NodesView
{
    public class ActionNodeView : NodeView
    {
        [HideInInspector]
        public override string StyleClassName { get; set; } = "action";

        public override void Initialize(BaseNode node, BehaviorTreeView view)
        {
            base.Initialize(node, view);

            // register to double clicks only if the node is a behavior tree runner node.
            if (node is BehaviorTreeNode)
            {
                RegisterCallback<MouseDownEvent>(OnMouseDown);
            }
        }

        protected override void CreateInputPort()
        {
            CreatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single);
        }

        protected override void CreateOutputPort()
        {

        }

        private void OnMouseDown(MouseDownEvent e)
        {
            // Check for double-click (2 clicks)
            if (e.clickCount == 2)
            {
                // Handle the double-click event for BehaviorTreeNode
                Debug.Log("Double-click detected on node: " + this.title);

                BehaviorTree bt = ((BehaviorTreeNode)Node).GetBehaviorTree();

                BehaviorTreeEditor.OpenBehaviorTree(bt);
            }
        }
    }
}
