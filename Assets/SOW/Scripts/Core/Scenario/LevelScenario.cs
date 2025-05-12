using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using SoW.Scripts.Core.Configs;
using SoW.Scripts.Core.Pool;
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

        [Button]
        private void Pass()
        {
            while (Preset.FoundedWords.Count < _levelData.words.Length)
            {
                Preset.FoundedWords.Add("aa");
            }
        }

        protected override void PlayInternal()
        {
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

            await UniTask.WaitUntil(() => Preset.FoundedWords.Count == _levelData.words.Length,
                cancellationToken: token);

            await UniTask.Delay(TimeSpan.FromSeconds(Data.Config.Levels.LevelPassDelay), cancellationToken: token);

            var levePassScenario = Data.Scenario.GetScenario<LevelPassScenario>().Play(new(Preset.LevelIndex), Token);

            Preset.LevelIndex++;
            SaveSystem.Saver.Save<LevelScenarioData>();

            await UniTask.WaitWhile(() => levePassScenario.IsPlaying, cancellationToken: token);
        }

        public async UniTask<float> GetBestLettersFitSize(string[] words, int maxGrids)
        {
            float bestFitSize = 0;
            float bestGridsCount = 0;

            List<WordsGrid> grids = new();

            for (int i = 1; i <= maxGrids; i++)
            {
                var nexGrid = _gameScreen.AddWordsGrid();
                grids.Add(nexGrid);

                Canvas.ForceUpdateCanvases();
                await UniTask.Yield();

                var lettersFitSize = _gameScreen.GetLettersFitSize(words, i);

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
                    var lettersToRemove =
                        _selectedLetters.Count - deselectedLetterIndex - 1; //-1 to save hovered letter selection

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
            _gameScreen.Hide();

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