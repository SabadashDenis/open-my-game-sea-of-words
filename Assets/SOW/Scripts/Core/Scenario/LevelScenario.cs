using System.Collections.Generic;
using System.Linq;
using SoW.Scripts.Core.Scenario._;

namespace SoW.Scripts.Core.Scenario
{
    public class LevelScenario : ScenarioBase
    {
        private GameScreen _gameScreen;
        
        private List<char> _currentInput = new();
        public string _lettersChain;

        private string[] _levelWords = { "канон", "икона", "цинк", "кино", "ион", "инок" };
        
        protected override void InitInternal(ScenarioData data)
        {
            _gameScreen = data.UI.GetScreen<GameScreen>();
        }

        protected override void PlayInternal()
        {
            SetupLettersChain(_levelWords);
            
            _gameScreen.Show();

            _gameScreen.InputCircle.OnLetterSelectionChanged += ProcessLetterInput;
            _gameScreen.InputCircle.OnInputFinished += ProcessInputFinish;
            
            _gameScreen.WordsGrid.SetupWords(_levelWords);
            _gameScreen.InputCircle.SetupLetters(_lettersChain);
        }

        private void SetupLettersChain(string[] words)
        {
            Dictionary<char, int> result = new();
            
            foreach (string word in words)
            {
                Dictionary<char, int> wordLetters = new();
                
                foreach (char letter in word)
                {
                    if (!wordLetters.TryAdd(letter, 1))
                    {
                        wordLetters[letter]++;
                    }
                }

                foreach (var wordLetter in wordLetters)
                {
                    if (result.TryGetValue(wordLetter.Key, out var resultLettersCount))
                    {
                        if(resultLettersCount < wordLetter.Value)
                            result[wordLetter.Key] = resultLettersCount;
                    }
                    else
                    {
                        result.Add(wordLetter.Key, wordLetter.Value);
                    }
                }
            }

            string lettersChain = string.Empty;

            foreach (var letterData in result)
            {
                for (int i = 0; i < letterData.Value; i++)
                {
                    lettersChain += letterData.Key;
                }
            }
            
            _lettersChain = lettersChain;
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

        private void ProcessInputFinish()
        { 
            var inputStr = new string(_currentInput.ToArray());
            
            if (_levelWords.Any(word => word == inputStr))
            {
                _gameScreen.WordsGrid.ShowWord(inputStr);
            }
            
            _currentInput.Clear();
            _gameScreen.InputResult.Clear();
        }
        
        protected override void OnTick()
        {
        }

        protected override void StopInternal()
        {
            _gameScreen.InputCircle.OnLetterSelectionChanged -= ProcessLetterInput;
            _gameScreen.InputCircle.OnInputFinished -= ProcessInputFinish;
        }
    }
}