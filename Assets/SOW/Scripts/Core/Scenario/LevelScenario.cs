using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using SoW.Scripts.Core.Configs;
using SoW.Scripts.Core.Save._;
using SoW.Scripts.Core.Scenario._;
using SoW.Scripts.Core.UI.Screen.Game.Views;
using SoW.Scripts.Core.Utility;
using SoW.Scripts.Core.Utility.Extended;
using UnityEngine;
using UnityEngine.UI;

namespace SoW.Scripts.Core.Scenario
{
    public class LevelScenario : AsyncScenarioBase<LevelScenarioData>
    {
        private GameScreen _gameScreen;
        private LevelData _levelData;

        private readonly List<SelectableLetterView> _selectedLetters = new();

        protected override void InitInternal(ScenarioData data)
        {
            _gameScreen = data.UI.GetScreen<GameScreen>();
        }

        protected override async UniTask AsyncPlayInternal(CancellationToken token)
        {
            _gameScreen.Show();
            _gameScreen.SetLevel(Preset.LevelIndex + 1);

            _levelData = Data.Config.Levels.GetData(Preset.LevelIndex);

            var bestLettersSize = await GetBestLettersFitSize(_levelData.words, Data.Config.Levels.MaxGrids);
            var lengthSortedWords = _levelData.words.OrderBy(x => x.Length).ToArray();
            _gameScreen.SetupWords(lengthSortedWords, bestLettersSize);

            var commonLetters = WordsUtility.GetCommonLetters(_levelData.words);
            _gameScreen.InputCircle.SetupLetters(commonLetters);
            
            Preset.FoundedWords.ForEach(word => _gameScreen.ShowWord(word, Token, true));
            
            Data.Input.Click.Released += ProcessInputFinish;

            foreach (var inputLetterView in _gameScreen.InputCircle.LetterViews)
            {
                inputLetterView.Hover += OnLetterHover;
                inputLetterView.Tap += OnLetterTap;
            }

            await UniTask.WaitUntil(() => Preset.FoundedWords.Count == _levelData.words.Length, cancellationToken: token);
            await UniTask.Delay(TimeSpan.FromSeconds(Data.Config.Levels.EndLevelDelay), cancellationToken: token);
            
            await Data.Scenario.GetScenario<LevelPassScenario>().Play(new(Preset.LevelIndex), token).WaitForEnd(token);
        }

        private async UniTask<float> GetBestLettersFitSize(string[] words, int maxGrids)
        {
            float bestFitSize = 0;
            float bestGridsCount = 0;

            List<WordsGrid> grids = new();

            for (int i = 1; i <= maxGrids; i++)
            {
                var newGrid = _gameScreen.AddWordsGrid();
                grids.Add(newGrid);

                Canvas.ForceUpdateCanvases();
                await UniTask.Yield();

                var lettersFitSize = _gameScreen.GetCurrentLettersFitSize(words);

                this.Log(LogType.Info, $"Letters fit size: {lettersFitSize}, Grids[{i}]");

                if (lettersFitSize > bestFitSize)
                {
                    bestFitSize = lettersFitSize;
                    bestGridsCount = i;
                }
            }

            if (bestGridsCount < maxGrids)
            {
                var gridsToRemove = maxGrids - bestGridsCount;

                for (int i = 0; i < gridsToRemove; i++)
                {
                    var lastGrid = grids.Last();
                    grids.Remove(lastGrid);
                    _gameScreen.RemoveGrid(lastGrid);
                }
            }

            this.Log(LogType.Info, $"Best fit size: {bestFitSize}, Grids[{bestGridsCount}]");

            return bestFitSize;
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
                        var lastLetter = _selectedLetters.Last();

                        lastLetter.SetSelected(false);
                        _selectedLetters.Remove(lastLetter);

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
            _selectedLetters.ForEach(letter => inputStr += letter.CurrentLetter);

            if (_levelData.words.Contains(inputStr) && !Preset.FoundedWords.Contains(inputStr))
            {
                _gameScreen.ShowWord(inputStr, Token);
                Preset.FoundedWords.Add(inputStr);
            }
            
            _gameScreen.InputCircle.ClearSelection();
            _gameScreen.InputResult.Clear();
            _selectedLetters.Clear();

            SaveSystem.Saver.Save<LevelScenarioData>();
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
            
            _gameScreen.Hide();
        }
    }

    [Serializable]
    public class LevelScenarioData
    {
        public int LevelIndex;
        public List<string> FoundedWords = new();

        public void ToNextLevel()
        {
            LevelIndex++;
            FoundedWords.Clear();
        }
    }
}