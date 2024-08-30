using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
    using Nodes;

    public class BehaviorTreeRunner : MonoBehaviour
    {
        public BehaviorTree Tree;

        private void Start()
        {
            Tree = Tree.Clone();
            // pass the controller of the B
            Tree.Bind(gameObject);
            Tree.Start();
        }

        private void Update()
        {
            Tree.Update();
        }
    }
}
