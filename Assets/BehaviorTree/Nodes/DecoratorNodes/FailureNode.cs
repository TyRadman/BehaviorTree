namespace BT.Nodes
{
    public class FailureNode : DecoratorNode
    {
        protected override NodeState OnUpdate()
        {
            return NodeState.Failure;
        }
    }
}
