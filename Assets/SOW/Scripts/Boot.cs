using SoW.Scripts.Core;
using SoW.Scripts.Core.Factory._;
using SoW.Scripts.Core.Scenario;
using SoW.Scripts.Core.Scenario._;
using UnityEngine;

public class Boot : MonoBehaviour
{
    [SerializeField] private UISystem uiSystem;
    [SerializeField] private ScenarioSystem scenarioSystem;
    [SerializeField] private PoolSystem poolSystem;
    
    private void Awake()
    {
        var systemData = new SystemData(uiSystem, scenarioSystem, poolSystem);
        
        uiSystem.Init(systemData);
        scenarioSystem.Init(systemData);
        poolSystem.Init(systemData);
        
        scenarioSystem.GetScenario<LevelScenario>().Play();
    }
}
