using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BT
{
    [CreateAssetMenu()]
    public class BehaviorTreeSettings : ScriptableObject
    {
        public static BehaviorTreeSettings Instance;
        public const string CORE_DIRECTORY = "Assets/BehaviorTree/";
        public const string PATH = "Assets/BehaviorTree/Settings/Settings.asset";
        [field: SerializeField] public BehaviorTree LastSelectedBehaviorTree { get; set; }
        [field: SerializeField] public List<BehaviorTree> RecentOpenedBehaviorTrees { get; set; }

        public void AddRecentBehaviorTree(BehaviorTree behaviorTree)
        {
            if (RecentOpenedBehaviorTrees.Contains(behaviorTree))
            {
                return;
            }

            RecentOpenedBehaviorTrees.Insert(0, behaviorTree);
        }

        public List<BehaviorTree> GetRecentBehaviorTrees()
        {
            return RecentOpenedBehaviorTrees;
        }

        public static BehaviorTreeSettings GetSettings()
        {
            if (Instance != null)
            {
                return Instance;
            }

            Instance = AssetDatabase.LoadAssetAtPath<BehaviorTreeSettings>(PATH);

            if (Instance == null)
            {
                Debug.LogError($"Couldn't find the settings in {PATH}");
                return null;
            }

            return Instance;
        }
    }
}
