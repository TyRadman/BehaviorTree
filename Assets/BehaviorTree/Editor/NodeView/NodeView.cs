using UnityEngine;
using UnityEditor.Experimental.GraphView;
using BT.Nodes;
using System;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace BT.NodesView
{
    public abstract class NodeView : Node
    {
        public Action<NodeView> OnNodeSelected;
        public BaseNode Node;
        public Port InputPort;
        public Port OutputPort;
        public abstract string StyleClassName { get; set; }
        private const string NODE_VIEW_UXML_DIRECTORY = BehaviorTreeSettings.CORE_DIRECTORY + "Editor/NodeView/NodeView.uxml";
        private Label _stateLabel;
        private bool _isRunning = false;
        private VisualElement _nodeBackground;

        public NodeView() : base(NODE_VIEW_UXML_DIRECTORY)
        {
        }

        public override void OnSelected()
        {
            base.OnSelected();

            OnNodeSelected?.Invoke(this);
        }

        #region Set up
        public virtual void Initialize(BaseNode node)
        {
            this.Node = node;
            this.viewDataKey = node.GUID;
            _stateLabel = this.Q<Label>("state-title");
            _stateLabel.visible = false;
            _nodeBackground = this.Q<VisualElement>("node-border");

            SetNodeInitialPosition();

            CreateInputPort();
            CreateOutputPort();

            VisualElement bg = this.Q<VisualElement>("title-background");

            if(bg == null)
            {
                Debug.LogError("No");
            }

            bg.AddToClassList(StyleClassName);

            BindTitleLabelToName();
            BindTooltipToDescription();
            //BindNodeColorChange();

        }

        #region Node coloring
        //private void BindNodeColorChange()
        //{
        //    RegisterCallback<DetachFromPanelEvent>(UnsubscribeFromBaseNode);
        //    //Node.ViewDetails.OnColorChanged += ChangeNodeColor;
        //}

        //private void ChangeNodeColor()
        //{
        //    _nodeBackground.style.backgroundColor = Node.ViewDetails.NodeColor;
        //}

        //private void UnsubscribeFromBaseNode(DetachFromPanelEvent evt)
        //{
        //    Debug.Log("Removed");
        //    //Node.ViewDetails.OnColorChanged -= ChangeNodeColor;
        //}
        #endregion

        private void SetNodeInitialPosition()
        {
            style.left = Node.Position.x;
            style.top = Node.Position.y;
        }

        private void BindTitleLabelToName()
        {
            Label titleLabel = this.Q<Label>("title-label");
            titleLabel.bindingPath = "ViewDetails.Name";
            titleLabel.Bind(new SerializedObject(Node));

            if (Node.ViewDetails.Name.Length == 0)
            {
                Node.ViewDetails.Name = Node.GetType().Name;
                titleLabel.text = Node.ViewDetails.Name;
            }
        }

        private void BindTooltipToDescription()
        {
            RegisterCallback<MouseEnterEvent>(UpdateTooltip);
        }

        private void UpdateTooltip(MouseEnterEvent evt)
        {
            tooltip = Node.ViewDetails.Description;
        }

        protected abstract void CreateInputPort();
        protected abstract void CreateOutputPort();
        #endregion

        #region Utilities
        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);

            Undo.RecordObject(Node, "Behavior Tree (Set Position)");

            Node.Position.x = newPos.xMin;
            Node.Position.y = newPos.yMin;

            EditorUtility.SetDirty(Node);
        }

        protected void CreatePort(Orientation orientation, Direction direction, Port.Capacity capacity)
        {
            Port port = InstantiatePort(orientation, direction, capacity, typeof(bool));

            // position and size the inner layer of the port
            VisualElement connector = port.Q<VisualElement>("connector");
            connector.pickingMode = PickingMode.Position;
            connector.style.height = 100;
            connector.style.width = 100;
            connector.style.borderBottomWidth = 0;
            connector.style.borderTopWidth = 0;

            if(direction == Direction.Output)
            {
                connector.style.borderBottomRightRadius = 0;
                connector.style.borderBottomLeftRadius = 0;
            }
            else
            {
                connector.style.borderTopRightRadius = 0;
                connector.style.borderTopLeftRadius = 0;
            }

            // position and size the outer layer of the port
            VisualElement cap = port.Q<VisualElement>("cap");
            cap.pickingMode = PickingMode.Position;
            cap.style.height = 100;
            cap.style.width = 100;

            if (direction == Direction.Output)
            {
                cap.style.borderBottomRightRadius = 0;
                cap.style.borderBottomLeftRadius = 0;
            }
            else
            {
                cap.style.borderTopRightRadius = 0;
                cap.style.borderTopLeftRadius = 0;
            }

            Label label = port.Q<Label>("type");
            label.RemoveFromHierarchy();

            if (direction == Direction.Input)
            {
                port.style.flexDirection = FlexDirection.Column;
                inputContainer.Add(port);
                InputPort = port;
            }
            else
            {
                port.style.flexDirection = FlexDirection.ColumnReverse;
                outputContainer.Add(port);
                OutputPort = port;
            }
        }

        #region Sort children based on position
        public void SortChildren()
        {
            CompositeNode composite = Node as CompositeNode;

            if (composite)
            {
                composite.Children.Sort(SortByHorizontalPosition);
            }
        }

        private int SortByHorizontalPosition(BaseNode left, BaseNode right)
        {
            return left.Position.x < right.Position.x ? -1 : 1;
        }
        #endregion
        #endregion


        /// <summary>
        /// Updates the border of the node depending on its state if the application is running
        /// </summary>
        public void UpdateState()
        {
            if (!Application.isPlaying)
            {
                if (_stateLabel.visible)
                {
                    _stateLabel.visible = false;
                }

                return;
            }

            if(Node.State != NodeState.Running)
            {
                _isRunning = false;
                RemoveFromClassList("running");
                _stateLabel.visible = false;
            }

            if (_isRunning)
            {
                return;
            }

            if (Node.State == NodeState.Running && Node._isStarted)
            {
                _isRunning = true;
                AddToClassList("running");
                _stateLabel.visible = true;
            }
        }

        //private void SubscribeToUpdate()
        //{
        //    EditorApplication.update += OnUpdate;
        //}

        //private void OnUpdate()
        //{
        //    if()
        //}
    }
}