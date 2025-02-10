namespace BT.Nodes
{
    using Utilities;

    public class RandomSequenceNode : SequenceNode
    {
        protected override void OnStart()
        {
            base.OnStart();

            BTHelper.ShuffleList(Children);
        }
    }
}