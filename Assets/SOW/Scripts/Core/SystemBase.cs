using SoW.Scripts.Core.Scenario._;
using SoW.Scripts.Core.Utility.Object.Initable;

namespace SoW.Scripts.Core
{
    public abstract class SystemBase : InitableBehaviour<SystemData> { }

    public struct SystemData
    {
        public readonly UISystem UI;
        public readonly ScenarioSystem Scenario;
        
        public SystemData(UISystem ui, ScenarioSystem scenario)
        {
            UI = ui;
            Scenario = scenario;
        }
    }
}