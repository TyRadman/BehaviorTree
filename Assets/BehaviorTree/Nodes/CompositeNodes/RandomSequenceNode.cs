namespace BT.Nodes
{
    using Utilities;

    [NodePath(COMPOSITE_NODE_PATH + "Random Sequence Node")]
    public class RandomSequenceNode : SequenceNode
    {
        protected override void OnStart()
        {
            base.OnStart();

            BTHelper.ShuffleList(Children);
        }
    }
}