using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using SoW.Scripts.Core.Save._;
using SoW.Scripts.Core.Scenario._;
using SoW.Scripts.Core.Utility.Extended;

namespace SoW.Scripts.Core.Scenario
{
    public class GameScenario : AsyncScenarioBase
    {
        private LevelScenario _levelScenario;
        private LevelPassScenario _levelPassScenario;
        
        protected override void InitInternal(ScenarioData data)
        {
            _levelScenario = data.Scenario.GetScenario<LevelScenario>();
            _levelPassScenario = data.Scenario.GetScenario<LevelPassScenario>();
        }

        protected override async UniTask AsyncPlayInternal(CancellationToken token)
        {
            var currentLevelData = SaveSystem.Saver.Get<LevelScenarioData>();
            
            this.Log(LogType.Info, $"Get Save! Level: [{currentLevelData.LevelIndex}], Words: [{currentLevelData.FoundedWords.Count}]");
            
            while (!Token.IsCancellationRequested)
            {
                await _levelScenario.Play(currentLevelData).WaitForEnd(token);
                
                currentLevelData.ToNextLevel();
                SaveSystem.Saver.Save<LevelScenarioData>();
                
                await _levelPassScenario.Play(new(currentLevelData.LevelIndex - 1), token).WaitForEnd(token);
            }
        }

        protected override void StopInternal()
        {
            _levelScenario.Stop();
        }
    }
}