using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using SoW.Scripts.Core.Configs;
using SoW.Scripts.Core.Save._;
using SoW.Scripts.Core.Scenario._;
using SoW.Scripts.Core.UI.Screen.Game.Views;
using SoW.Scripts.Core.Utility;
using UnityEngine;

namespace SoW.Scripts.Core.Scenario
{
    public class LevelScenario : AsyncScenarioBase<LevelScenarioData>
    {
        private GameScreen _gameScreen;
        private LevelData _levelData;
        
        private List<SelectableLetterView> _selectedLetters = new();
        
        protected override void InitInternal(ScenarioData data)
        {
            _gameScreen = data.UI.GetScreen<GameScreen>();
        }

        protected override void PlayInternal() { }

        protected override async UniTask AsyncPlayInternal(CancellationToken token)
        {
            var levelsData = Data.Config.Levels.Data.data;
            _levelData = levelsData[Preset.LevelIndex % levelsData.Count];
            
            _gameScreen.Show();
            
            _gameScreen.SetLevel(Preset.LevelIndex + 1);
            
            var bestLettersSize = await _gameScreen.GetBestLettersFitSize(_levelData.words, 3);
            var lengthSortedWords = _levelData.words.OrderBy(x => x.Length).ToArray();
            _gameScreen.SetupWords(lengthSortedWords, bestLettersSize);
            
            var letters = WordsUtility.GetCommonLetters(_levelData.words);
            _gameScreen.InputCircle.SetupLetters(letters);

            foreach (var foundedWord in Preset.FoundedWords)
            {
                _gameScreen.ShowWord(foundedWord, true);
            }
            
            Data.Input.Click.Released += ProcessInputFinish;
            
            foreach (var inputLetterView in _gameScreen.InputCircle.LetterViews)
            {
                inputLetterView.Hover += OnLetterHover;
                inputLetterView.Tap += OnLetterTap;
            }
            await UniTask.WaitUntil(() => Preset.FoundedWords.Count == _levelData.words.Length, cancellationToken: token);
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
            
            if (_levelData.words.Any(word => word == inputStr) && !Preset.FoundedWords.Contains(inputStr))
            {
                _gameScreen.ShowWord(inputStr);
                Preset.FoundedWords.Add(inputStr);
                this.Log(LogType.Info, $"FoundedWords: {Preset.FoundedWords.Count}/{_levelData.words.Length}");
            }
            
            _selectedLetters.Clear();
            _gameScreen.InputCircle.ClearSelection();
            _gameScreen.InputResult.Clear();
            
            SaveSystem.Saver.Save<LevelScenarioData>();
            this.Log(LogType.Info, $"Save! Level: [{SaveSystem.Saver.Get<LevelScenarioData>().LevelIndex}], " +
                                   $"Words: [{SaveSystem.Saver.Get<LevelScenarioData>().FoundedWords.Count}]");
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
            Preset.FoundedWords.Clear();
        }
    }

    [Serializable]
    public class LevelScenarioData
    {
        public int LevelIndex;
        public List<string> FoundedWords = new();
    }
}