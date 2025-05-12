using SoW.Scripts.Core;
using SoW.Scripts.Core.Configs._;
using SoW.Scripts.Core.Input._;
using SoW.Scripts.Core.Save._;
using SoW.Scripts.Core.Scenario;
using SoW.Scripts.Core.Scenario._;
using UnityEngine;

public class Boot : MonoBehaviour
{
    [SerializeField] private UISystem uiSystem;
    [SerializeField] private ScenarioSystem scenarioSystem;
    [SerializeField] private PoolSystem poolSystem;
    [SerializeField] private InputSystem inputSystem;
    [SerializeField] private ConfigSystem configSystem;
    [SerializeField] private SaveSystem saveSystem;
    
    private void Awake()
    {
        var systemData = new SystemData(uiSystem, scenarioSystem, poolSystem, inputSystem, configSystem);
        
        saveSystem.Init(systemData);
        poolSystem.Init(systemData);
        uiSystem.Init(systemData);
        inputSystem.Init(systemData);
        configSystem.Init(systemData);
        scenarioSystem.Init(systemData);
        
        scenarioSystem.GetScenario<GameScenario>().Play();
    }

    private void OnDestroy()
    {
        SaveSystem.Saver.SaveAll();
    }
}
