using SoW.Scripts.Core.Scenario._;

namespace SoW.Scripts.Core.Scenario
{
    public class GameScenario : ScenarioBase
    {
        private GameScreen _gameScreen;
        
        protected override void InitInternal(ScenarioData data)
        {
            _gameScreen = data.UI.GetScreen<GameScreen>();
        }

        protected override void PlayInternal()
        {
            _gameScreen.Show();
        }

        protected override void OnTick()
        {
        }

        protected override void StopInternal()
        {
        }
    }
}