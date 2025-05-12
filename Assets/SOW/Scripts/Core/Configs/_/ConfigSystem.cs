using UnityEngine;

namespace SoW.Scripts.Core.Configs._
{
    public class ConfigSystem : SystemBase
    {
        [SerializeField] private LevelsConfig levelsConfig;
        
        public LevelsConfig Levels => levelsConfig;
        
        protected override void InitInternal(SystemData data)
        {

        }
    }
}