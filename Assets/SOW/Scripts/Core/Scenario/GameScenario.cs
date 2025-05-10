using System.Threading;
using Cysharp.Threading.Tasks;
using SoW.Scripts.Core.Scenario._;

namespace SoW.Scripts.Core.Scenario
{
    public class GameScenario : AsyncScenarioBase
    {
        private LevelScenario _levelScenario;

        private int _currentLevelIndex;
        
        protected override void InitInternal(ScenarioData data)
        {
            _levelScenario = data.Scenario.GetScenario<LevelScenario>();
        }

        protected override async UniTask AsyncPlayInternal(CancellationToken token)
        {
            while (!Token.IsCancellationRequested)
            {
                _levelScenario.Play(_currentLevelIndex);
                await UniTask.WaitWhile(() => _levelScenario.IsPlaying, cancellationToken: Token);
                _currentLevelIndex++;
            }
        }

        protected override void PlayInternal() { }

        protected override void StopInternal()
        {
            _levelScenario.Stop();
        }
    }
}