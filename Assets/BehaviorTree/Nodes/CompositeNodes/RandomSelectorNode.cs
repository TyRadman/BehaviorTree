namespace BT.Nodes
{
    using Utilities;

    public class RandomSelectorNode : SelectorNode
    {
        protected override void OnStart()
        {
            base.OnStart();

            BTHelper.ShuffleList(Children);
        }
    }
}