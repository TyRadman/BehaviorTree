namespace BT.Nodes
{
    public class SucceederNode : DecoratorNode
    {
        protected override NodeState OnUpdate()
        {
            return NodeState.Success;
        }
    }
}
