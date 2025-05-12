using System.Threading;
using Cysharp.Threading.Tasks;
using SoW.Scripts.Core.Scenario._;
using SoW.Scripts.Core.UI.Screen.Win;

namespace SoW.Scripts.Core.Scenario
{
    public class LevelPassScenario : AsyncScenarioBase<LevelPassScenarioData>
    {
        private WinScreen _winScreen;

        private bool _nextLvlClicked;
        
        protected override void InitInternal(ScenarioData data)
        {
            _winScreen = Data.UI.GetScreen<WinScreen>();
        }

        protected override async UniTask AsyncPlayInternal(CancellationToken token)
        {
            _nextLvlClicked = false;
            
            _winScreen.NextLvlBtn.OnClickEvent += ToNextLevel;

            _winScreen.Show();
            _winScreen.SetupTexts(Preset.PassedLevelIndex + 1);
            
            await UniTask.WaitUntil(() => _nextLvlClicked, cancellationToken: token);
        }

        private void ToNextLevel() => _nextLvlClicked = true;
        
        protected override void StopInternal()
        {
            _winScreen.NextLvlBtn.OnClickEvent -= ToNextLevel;
            
            _winScreen.Hide();
        }
    }

    public struct LevelPassScenarioData
    {
        public readonly int PassedLevelIndex;

        public LevelPassScenarioData(int passedLevelIndex)
        {
            PassedLevelIndex = passedLevelIndex;
        }
    }
}