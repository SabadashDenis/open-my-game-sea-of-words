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
        
        protected override void InitInternal(ScenarioData data)
        {
            _levelScenario = data.Scenario.GetScenario<LevelScenario>();
        }

        protected override async UniTask AsyncPlayInternal(CancellationToken token)
        {
            var savedLevelData = SaveSystem.Saver.Get<LevelScenarioData>();
            
            this.Log(LogType.Info, $"Get Save! Level: [{savedLevelData.LevelIndex}], Words: [{savedLevelData.FoundedWords.Count}]");
            
            while (!Token.IsCancellationRequested)
            {
                await _levelScenario.Play(savedLevelData).WaitForEnd(Token);
            }
        }

        protected override void PlayInternal() { }

        protected override void StopInternal()
        {
            _levelScenario.Stop();
        }
    }
}