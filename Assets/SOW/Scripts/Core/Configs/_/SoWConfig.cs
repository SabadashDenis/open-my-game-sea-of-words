using Sirenix.OdinInspector;
using SoW.Scripts.Core.Factory._;
using UnityEngine;

namespace SoW.Scripts.Core.Configs._
{
    [CreateAssetMenu(menuName = "SoW/SoWConfig")]
    public class SoWConfig : SerializedScriptableObject
    {
        [SerializeField] private FactoryConfig factory = new();
        
        public FactoryConfig Factory => factory;
    }
}