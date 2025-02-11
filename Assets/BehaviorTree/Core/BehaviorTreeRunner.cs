using BT;
using UnityEngine;

namespace BT
{
    public class BehaviorTreeRunner : MonoBehaviour
    {
        public BehaviorTree Tree;

        [SerializeField] private bool _runOnStart = false;
        [SerializeField] private MonoBehaviour _agent;

        private bool _isRunning = false;

        private void Start()
        {
            if (_runOnStart)
            {
                Run(_agent);
            }
        }

        public void Run(MonoBehaviour agent = null)
        {
            if (_agent == null && agent == null)
            {
                Debug.LogError($"No agent found on {gameObject.name}");
                return;
            }

            if (agent == null)
            {
                agent = _agent;
            }

            Tree = Tree.Clone();
            Tree.Bind(agent);
            Tree.Start();

            _isRunning = true;
        }

        public void Stop()
        {
            _isRunning = false;
        }

        private void Update()
        {
            if (!_isRunning)
            {
                return;
            }

            Tree.Update();
        }
    }
}