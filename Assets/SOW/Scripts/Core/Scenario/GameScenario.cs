using System;
using System.Data;
using System.Threading;
using Cysharp.Threading.Tasks;
using SoW.Scripts.Core.Save._;
using SoW.Scripts.Core.Scenario._;

namespace SoW.Scripts.Core.Scenario
{
    public class GameScenario : AsyncScenarioBase
    {
        private LevelScenario _levelScenario;
        private LevelPassScenario _levelPassScenario;
        
        private LevelScenarioData _currentLevelData;
        
        protected override void InitInternal(ScenarioData data)
        {
            _levelScenario = data.Scenario.GetScenario<LevelScenario>();
            _levelPassScenario = data.Scenario.GetScenario<LevelPassScenario>();
            
            _currentLevelData = SaveSystem.Saver.Get<LevelScenarioData>();
            this.Log(LogType.Info, $"Get Save! Level: [{_currentLevelData.LevelIndex}], Words: [{_currentLevelData.FoundedWords.Count}]");
        }

        protected override async UniTask AsyncPlayInternal(CancellationToken token)
        {
            
            while (!Token.IsCancellationRequested)
            {
                _levelScenario.Play(_currentLevelData);
                await UniTask.WaitWhile(() => _levelScenario.IsPlaying, cancellationToken: Token);
                
                _levelPassScenario.Play(new (_currentLevelData.LevelIndex));
                await UniTask.WaitWhile(() => _levelPassScenario.IsPlaying, cancellationToken: Token);
                
                _currentLevelData.LevelIndex++;
            }
        }

        protected override void PlayInternal() { }

        protected override void StopInternal()
        {
            _levelScenario.Stop();
        }
    }
}