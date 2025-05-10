using SoW.Scripts.Core.Configs._;
using SoW.Scripts.Core.Input._;
using SoW.Scripts.Core.Scenario._;
using SoW.Scripts.Core.Utility.Object.Initable;

namespace SoW.Scripts.Core
{
    public abstract class SystemBase : InitableBehaviour<SystemData> { }

    public struct SystemData
    {
        public readonly UISystem UI;
        public readonly ScenarioSystem Scenario;
        public readonly PoolSystem Pool;
        public readonly InputSystem Input;
        public readonly ConfigSystem Config;
        
        public SystemData(UISystem ui, ScenarioSystem scenario, PoolSystem pool, InputSystem input, ConfigSystem config)
        {
            UI = ui;
            Scenario = scenario;
            Pool = pool;
            Input = input;
            Config = config;
        }
    }
}