using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SoW.Scripts.Core.Scenario._
{
    public class ScenarioSystem : SystemBase
    {
        [SerializeField] private List<ScenarioBase> scenarios = new();
        
        protected override void InitInternal(SystemData data)
        {
            foreach (var scenario in scenarios)
            {
                scenario.Init(new(data.UI, data.Pool, data.Input));
            }
        }
        
        public TScenario GetScenario<TScenario>() where TScenario : ScenarioBase
        {
            if (scenarios.Count > 0)
            {
                var resultScenario = scenarios.FirstOrDefault(scenario => scenario is TScenario);
            
                if(resultScenario != null)
                    return resultScenario as TScenario;
            }
        
            this.Log(LogType.Error, $"Can't get scenario of type {typeof(TScenario).Name}");
            return null;
        }
        
        [Button]
        private void Collect() => scenarios = GetComponentsInChildren<ScenarioBase>().ToList();
    }
}