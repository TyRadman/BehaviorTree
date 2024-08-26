using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
    [CreateAssetMenu()]
    public class BehaviorTreeSettings : ScriptableObject
    {
        public const string CORE_DIRECTORY = "Assets/BehaviorTree/";
        public const string PATH = "Assets/BehaviorTree/Settings/Settings.asset";
        [field: SerializeField] public BehaviorTree LastSelectedBehaviorTree { get; set; }
    }
}
