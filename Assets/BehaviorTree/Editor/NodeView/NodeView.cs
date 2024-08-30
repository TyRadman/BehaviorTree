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
        private VisualElement _nodeBackground;
        private static BehaviorTreeView _view;
        private static NodeViewPort _port;
        private NodeState _currentState = NodeState.NONE;
        private string _lastAddedClass = string.Empty;

        public NodeView() : base(NODE_VIEW_UXML_DIRECTORY)
        {
        }

        public override void OnSelected()
        {
            base.OnSelected();

            OnNodeSelected?.Invoke(this);
        }

        #region Set up
        public virtual void Initialize(BaseNode node, BehaviorTreeView view)
        {
            _view = view;
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

        #region Ports
        protected void CreatePort(Orientation orientation, Direction direction, Port.Capacity capacity)
        {
            new NodeViewPort(this, _view, orientation, direction, capacity);
        }
        #endregion

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

            if (Node.State == _currentState)
            {
                return;
            }

            RemoveFromClassList(_lastAddedClass);

            NodeState state = Node.State;

            switch(state)
            {
                case NodeState.Running:
                    _currentState = state;
                    _lastAddedClass = "running";
                    AddToClassList(_lastAddedClass);
                    _stateLabel.text = "running";
                    _stateLabel.visible = true;
                    break;
                case NodeState.Failure:
                    _currentState = state;
                    _lastAddedClass = "failure";
                    _stateLabel.text = "failure";
                    AddToClassList(_lastAddedClass);
                    _stateLabel.visible = true;
                    break;
                case NodeState.Success:
                    _currentState = state;
                    _lastAddedClass = "success";
                    _stateLabel.text = "success";
                    AddToClassList(_lastAddedClass);
                    _stateLabel.visible = true;
                    break;
            }

            //if(Node.State != NodeState.Running)
            //{
            //    _isRunning = false;
            //    RemoveFromClassList("running");
            //    _stateLabel.visible = false;
            //}

            //if (_isRunning)
            //{
            //    return;
            //}

            //if (Node.State == NodeState.Running && Node.IsStarted)
            //{
            //    _isRunning = true;
            //    AddToClassList("running");
            //    _stateLabel.visible = true;
            //}
        }
    }

    public class CustomEdgeConnectorListener : IEdgeConnectorListener
    {
        private bool _connectionSuccessful;

        public bool ConnectionSuccessful => _connectionSuccessful;

        public void OnDropOutsidePort(Edge edge, Vector2 position)
        {
            // The drop was outside a port, so the connection is not successful
            _connectionSuccessful = false;
        }

        public void OnDrop(GraphView graphView, Edge edge)
        {
            // The drop was onto a valid port, check if both ends are connected
            if (edge.input != null && edge.output != null)
            {
                _connectionSuccessful = true;
            }
            else
            {
                _connectionSuccessful = false;
            }
        }
    }

}