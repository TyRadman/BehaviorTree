using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT.Nodes
{
#if UNITY_EDITOR
    public class WaitNode : ActionNode
    {
        public float Duration = 1f;
        private float _elapsedTime;

        protected override void OnStart()
        {
            _elapsedTime = 0f;
        }

        protected override void OnExit()
        {
           
        }

        protected override NodeState OnUpdate()
        {
            if (_elapsedTime < Duration)
            {
                _elapsedTime += Time.deltaTime;
                return NodeState.Running;
            }

            return NodeState.Success;
        }
    }
#endif
}
