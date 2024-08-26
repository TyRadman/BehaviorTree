using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BT
{
    using Nodes;

    [System.Serializable]
    public class NodeSearchDirectory
    {
        [HideInInspector] public string ClassName;
        public string Directory;
        public Type ClassType;
    }

    [CreateAssetMenu()]
    public class BehaviorTreeSearchDirectories : ScriptableObject
    {
        [field: SerializeField] public List<NodeSearchDirectory> Directories { get; private set; } = new List<NodeSearchDirectory>();
        private List<Type> DefaultNodes = new List<Type>
        {
            typeof(PrintNode), typeof(WaitNode), typeof(SequenceNode), 
            typeof(RepeatNode), typeof(SelectorNode),
            typeof(LoopNode), typeof(InvertNode)
            
        };

        public void RefreshDictionary()
        {
            TypeCache.TypeCollection nodeTypes = TypeCache.GetTypesDerivedFrom(typeof(BaseNode));

            for (int i = Directories.Count - 1; i >= 0; i--)
            {
                string[] names = Directories[i].Directory.Split('/');
                string className = names.Last();

                if(nodeTypes.ToList().Exists(c => c == Directories[i].ClassType))
                {
                    Directories.Remove(Directories[i]);
                }
            }

            foreach (Type node in nodeTypes)
            {
                if(Directories.Exists(d => d.ClassName == node.Name) || node.IsAbstract || node == typeof(RootNode))
                {
                    continue;
                }

                string directory = DefaultNodes.Exists(n => n == node) ? "Defaults/" : "Custom/";
                string childDirectory = $"{ObjectNames.NicifyVariableName(node.BaseType.Name + "s")}/{ObjectNames.NicifyVariableName(node.Name)}";

                Directories.Add(new NodeSearchDirectory()
                {
                    ClassName = node.Name,
                    Directory = directory + childDirectory,
                    ClassType = node
                });
            }
        }

        public void ResetDictionary()
        {
            Directories.Clear();
        }
    }
}
