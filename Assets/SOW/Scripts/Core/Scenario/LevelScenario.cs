using System.Collections.Generic;
using SoW.Scripts.Core.Scenario._;
using SoW.Scripts.Core.UI.Screen.Game.Views;

namespace SoW.Scripts.Core.Scenario
{
    public class LevelScenario : ScenarioBase
    {
        private GameScreen _gameScreen;
        private List<char> _currentInput = new();
        
        protected override void InitInternal(ScenarioData data)
        {
            _gameScreen = data.UI.GetScreen<GameScreen>();
        }

        protected override void PlayInternal()
        {
            _gameScreen.Show();

            _gameScreen.InputCircle.OnInputChanged += ProcessLetterInput;
            
            _gameScreen.WordsGrid.SetupWords(new[]{"канон","икона","цинк","кино","ион","инок"});
            _gameScreen.InputCircle.SetupLetters(_gameScreen.WordsGrid.LettersChain);
        }

        private void ProcessLetterInput(char letter, bool needToAdd)
        {
            if (needToAdd)
            {
                _currentInput.Add(letter);
                _gameScreen.InputResult.Append(letter);
            }
            else
            {
                _currentInput.Remove(letter);
                _gameScreen.InputResult.RemoveLast();
            }
        }
        
        protected override void OnTick()
        {
        }

        protected override void StopInternal()
        {
            _gameScreen.InputCircle.OnInputChanged -= ProcessLetterInput;
        }
    }
}