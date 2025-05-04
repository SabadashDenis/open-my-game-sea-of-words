using SoW.Scripts.Core;
using SoW.Scripts.Core.Scenario;
using SoW.Scripts.Core.Scenario._;
using UnityEngine;

public class Boot : MonoBehaviour
{
    [SerializeField] private UISystem uiSystem;
    [SerializeField] private ScenarioSystem scenarioSystem;
    
    private void Awake()
    {
        var systemData = new SystemData(uiSystem, scenarioSystem);
        
        uiSystem.Init(systemData);
        scenarioSystem.Init(systemData);
        
        scenarioSystem.GetScenario<GameScenario>().Play();
    }
}
