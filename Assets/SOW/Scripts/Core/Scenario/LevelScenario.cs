using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using SoW.Scripts.Core.Configs;
using SoW.Scripts.Core.Scenario._;
using SoW.Scripts.Core.UI.Screen.Game.Views;

namespace SoW.Scripts.Core.Scenario
{
    public class LevelScenario : AsyncScenarioBase<LevelScenarioData>
    {
        private GameScreen _gameScreen;
        private LevelData _levelData;
        
        private List<SelectableLetterView> _selectedLetters = new();
        private List<string> _foundedWords = new();
        
        protected override void InitInternal(ScenarioData data)
        {
            _gameScreen = data.UI.GetScreen<GameScreen>();
        }

        protected override void PlayInternal() { }

        protected override async UniTask AsyncPlayInternal(CancellationToken token)
        {
            var levelsData = Data.Config.Levels.Data.data;
            _levelData = levelsData[Preset.LevelIndex % levelsData.Count];
            
            GetLettersChain(_levelData.words);
            
            _gameScreen.Show();
            
            var bestLettersSize = await _gameScreen.GetBestLettersFitSize(_levelData.words, Preset.MaxGridsCount);
            var lengthSortedWords = _levelData.words.OrderBy(x => x.Length).ToArray();
            
            _gameScreen.SetLevel(Preset.RealLevelNumber);
            _gameScreen.SetupWords(lengthSortedWords, bestLettersSize);
            _gameScreen.InputCircle.SetupLetters(GetLettersChain(_levelData.words));
            
            Data.Input.Click.Released += ProcessInputFinish;
            
            foreach (var inputLetterView in _gameScreen.InputCircle.LetterViews)
            {
                inputLetterView.Hover += OnLetterHover;
                inputLetterView.Tap += OnLetterTap;
            }
            await UniTask.WaitUntil(() => _foundedWords.Count == _levelData.words.Length, cancellationToken: token);
        }

        private string GetLettersChain(string[] words)
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
                            result[wordLetter.Key] = wordLetter.Value;
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
            
            return lettersChain;
        }

        private void OnLetterHover(SelectableLetterView letterView)
        {
            if (Data.Input.Click.Started && _selectedLetters.Count > 0)
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
            
            if (_levelData.words.Any(word => word == inputStr) && !_foundedWords.Contains(inputStr))
            {
                _gameScreen.ShowWord(inputStr);
                _foundedWords.Add(inputStr);
                this.Log(LogType.Info, $"FoundedWords: {_foundedWords.Count}/{_levelData.words.Length}");
            }
            
            _selectedLetters.Clear();
            _gameScreen.InputCircle.ClearSelection();
            _gameScreen.InputResult.Clear();
        }
        
        protected override void StopInternal()
        {
            foreach (var inputLetterView in _gameScreen.InputCircle.LetterViews)
            {
                inputLetterView.Hover -= OnLetterHover;
                inputLetterView.Tap -= OnLetterTap;
            }
            
            Data.Input.Click.Released -= ProcessInputFinish;
            
            _gameScreen.Clear();
            
            _selectedLetters.Clear();
            _foundedWords.Clear();
        }
    }

    public struct LevelScenarioData
    {
        public readonly int LevelIndex;
        public readonly int RealLevelNumber;
        public readonly int MaxGridsCount;

        public LevelScenarioData(int levelIndex, int realLevelNumber, int maxGridsCount)
        {
            LevelIndex = levelIndex;
            RealLevelNumber = realLevelNumber;
            MaxGridsCount = maxGridsCount;
        }

    }
}