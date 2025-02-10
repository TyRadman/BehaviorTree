using UnityEngine;

namespace BT
{
    public class BehaviorTreeRunner : MonoBehaviour
    {
        public BehaviorTree Tree;

        [SerializeField] private bool _runOnStart = false;
        [SerializeField] private MonoBehaviour _agent;

        private void Start()
        {
            if (_runOnStart)
            {
                Run(_agent);
            }
        }

        public void Run(MonoBehaviour agent)
        {
            Tree = Tree.Clone();
            Tree.Bind(agent);
            Tree.Start();
        }

        private void Update()
        {
            Tree.Update();
        }
    }
}
