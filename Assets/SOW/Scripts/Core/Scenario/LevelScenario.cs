using System.Collections.Generic;
using System.Linq;
using SoW.Scripts.Core.Scenario._;
using SoW.Scripts.Core.UI.Screen.Game.Views;

namespace SoW.Scripts.Core.Scenario
{
    public class LevelScenario : ScenarioBase
    {
        private GameScreen _gameScreen;
        
        private List<SelectableLetterView> _selectedLetters = new();
        
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

            _gameScreen.WordsGrid.SetupWords(_levelWords);
            _gameScreen.InputCircle.SetupLetters(_lettersChain);
            
            Data.Input.Tap.Released += ProcessInputFinish;
            
            foreach (var inputLetterView in _gameScreen.InputCircle.LetterViews)
            {
                inputLetterView.Hover += OnLetterHover;
                inputLetterView.Tap += OnLetterTap;
            }
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

        private void OnLetterHover(SelectableLetterView letterView)
        {
            if (Data.Input.Tap.Started && _selectedLetters.Count > 0)
            {
                if (letterView.IsSelected)
                {
                    var deselectedLetterIndex = _selectedLetters.IndexOf(letterView);
                    var lettersToRemove = _selectedLetters.Count - deselectedLetterIndex - 1; //-1 to save hovered letter selection
                    
                    for (int i = 0; i < lettersToRemove; i++)
                    {
                        var lastLetterIndex = _selectedLetters.Count - 1;
                        
                        _selectedLetters[lastLetterIndex].SetSelected(false);
                        _selectedLetters.RemoveAt(lastLetterIndex);
                        
                        _gameScreen.InputResult.RemoveLast();
                    }
                }
                else
                {
                    letterView.SetSelected(true);
                    _gameScreen.InputResult.Append(letterView.CurrentLetter);
                    _selectedLetters.Add(letterView);
                }
            }
        }
        
        private void OnLetterTap(SelectableLetterView letterView)
        {
            if (_selectedLetters.Count == 0)
            {
                letterView.SetSelected(true);
                _gameScreen.InputResult.Append(letterView.CurrentLetter);
                _selectedLetters.Add(letterView);
            }
        }
        
        private void ProcessInputFinish()
        {
            string inputStr = string.Empty;

            foreach (var selectedLetter in _selectedLetters)
            {
                inputStr += selectedLetter.CurrentLetter;
            }
            
            if (_levelWords.Any(word => word == inputStr))
            {
                _gameScreen.WordsGrid.ShowWord(inputStr);
            }
            
            _selectedLetters.Clear();
            _gameScreen.InputCircle.ClearSelection();
            _gameScreen.InputResult.Clear();
        }
        
        protected override void OnTick()
        {
        }

        protected override void StopInternal()
        {
            foreach (var inputLetterView in _gameScreen.InputCircle.LetterViews)
            {
                inputLetterView.Hover -= OnLetterHover;
                inputLetterView.Tap -= OnLetterTap;
            }
            
            Data.Input.Tap.Released -= ProcessInputFinish;
        }
    }
}