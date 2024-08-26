using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BT.Nodes
{
    public abstract class BaseNode : ScriptableObject
    {
        [HideInInspector]
        public NodeState State = NodeState.Running;
        [HideInInspector]
        public bool _isStarted = false;
        [HideInInspector]
        public BlackboardVariablesContainer Blackboard;
        [HideInInspector]
        public GameObject Agent;

#if UNITY_EDITOR
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
#endif

        public NodeState Update()
        {
            if(!_isStarted)
            {
                OnStart();
                _isStarted = true;
            }

            State = OnUpdate();

            // if the node fails or succeeds, stop it
            if(State == NodeState.Failure || State == NodeState.Success)
            {
                OnStop();
                _isStarted = false;
            }

            return State;
        }

        protected abstract void OnStart();
        protected abstract void OnStop();
        protected abstract NodeState OnUpdate();

        /// <summary>
        /// Called when the node is force-stopped
        /// </summary>
        public abstract void StopNode();

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
        /// Returns a copy of this node. (Runtime copy)
        /// </summary>
        /// <returns>The copy of the node.</returns>
        public virtual BaseNode Clone()
        {
            return Instantiate(this);
        }
    }
}
