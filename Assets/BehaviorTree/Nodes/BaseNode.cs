using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace BT.Nodes
{
    public abstract class BaseNode : ScriptableObject, IDisposable
    {
        [HideInInspector]
        public NodeState State = NodeState.NONE;
        [HideInInspector]
        public bool IsStarted = false;
        [HideInInspector]
        public BlackboardVariablesContainer Blackboard { get; set; }
        [HideInInspector]
        public MonoBehaviour Agent;

        public BehaviorTree BehaviorTree { get; set; }

        //#if UNITY_EDITOR
        [HideInInspector]
        public string GUID;
        [HideInInspector]
        public Vector2 Position;

        [System.Serializable]
        public class NodeViewDetails
        {
            public string Name;
            [TextArea] public string Description;
        }

        public NodeViewDetails ViewDetails = new NodeViewDetails();

        /// <summary>
        /// The variable name that will allow access to this node from outside the BT system
        /// </summary>
        public string VariableName;

        protected const string MAIN_NODE_PATH = "Default Nodes/";

        public virtual string GetNodeViewName()
        {
            return "Node Name";
        }

        public virtual void ClearChildren()
        {

        }
//#endif

        public T GetAgent<T>() where T : MonoBehaviour
        {
            return Agent as T;
        }

        public NodeState Update()
        {
            if(!IsStarted)
            {
                OnStart();
                IsStarted = true;
            }

            State = OnUpdate();

            // if the node fails or succeeds, stop it
            if (State == NodeState.Failure || State == NodeState.Success)
            {
                OnExit();

                if (State == NodeState.Failure)
                {
                    OnForceStopNode();
                }

                IsStarted = false;
            }

            return State;
        }

        public abstract void OnAwake();
        protected abstract void OnStart();
        protected abstract void OnExit();
        protected abstract NodeState OnUpdate();

        /// <summary>
        /// Called when the node is force-stopped
        /// </summary>
        public virtual void OnForceStopNode()
        {
            IsStarted = false;
        }

        public virtual void AddChild(BaseNode child)
        {
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssetIfDirty(this);
        }

        public virtual void RemoveChild(BaseNode child)
        {
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssetIfDirty(this);
        }

        public virtual List<BaseNode> GetChildren()
        {
            return null;
        }

        public virtual bool HasChildren()
        {
            return false;
        }

        /// <summary>
        /// Returns a copy of the node (Runtime copy) and clones its children if applicable.
        /// </summary>
        /// <returns>The copy of the node.</returns>
        public virtual BaseNode Clone()
        {
            State = NodeState.NONE;
            // Called when the nodes are created
            return Instantiate(this);
        }

        public virtual void Dispose()
        {

        }
    }
}
