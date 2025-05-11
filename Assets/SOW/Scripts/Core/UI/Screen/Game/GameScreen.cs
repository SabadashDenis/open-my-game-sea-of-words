using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using SoW.Scripts.Core;
using SoW.Scripts.Core.UI;
using SoW.Scripts.Core.UI.Screen.Game.Views;
using SoW.Scripts.Core.Utility.Extended;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using LogType = SoW.Scripts.Core.LogType;

public class GameScreen : ScreenViewBase
{
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private HorizontalLayoutGroup layoutGroup;
    [SerializeField] private WordsGrid wordGridPrefab;
    [SerializeField] private LetterInputCircle inputCircle;
    [SerializeField] private LetterInputResult inputResult;

    private Dictionary<string, WordsGridLine> _words = new();
    private List<WordsGrid> _wordGrids = new();

    public LetterInputCircle InputCircle => inputCircle;
    public LetterInputResult InputResult => inputResult;

    public void SetLevel(int level)
    {
        levelText.text = $"Уровень {level}";
    }
    
    public void SetupWords(string[] words, float letterSize)
    {
        var wordPacks = ListExtensions.SplitList(words.ToList(), _wordGrids.Count);

        List<LetterView> allLetterViews = new();

        for (int i = 0; i < wordPacks.Count; i++)
        {
            var targetWordsGrid = _wordGrids[i];

            targetWordsGrid.SetupWords(wordPacks[i].ToArray());
            _words.AddRange(targetWordsGrid.Words);

            foreach (var wordLine in targetWordsGrid.Words.Values)
            {
                allLetterViews.AddRange(wordLine.Letters);
            }
        }
        
        foreach (var letterView in allLetterViews)
        {
            letterView.SetSize(letterSize);
        }
    }

    public void Clear()
    {
        foreach (var grid in _wordGrids)
        {
            grid.Clear();
            Destroy(grid.gameObject);
        }
        
        _wordGrids.Clear();
        _words.Clear();
    }

    public async UniTask<float> GetBestLettersFitSize(string[] words, int maxGrids)
    {
        float bestFitSize = 0f;
        float bestGridsCount = 0;
        
        for (int i = 0; i < maxGrids; i++)
        {
            AddWordsGrid();

            await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
            
            var lettersFitSize = GetLettersFitSize(words, i + 1);

            this.Log(LogType.Info, $"Letters fit size: {lettersFitSize}, Grids[{i + 1}]");

            if (lettersFitSize > bestFitSize)
            {
                bestFitSize = lettersFitSize;
                bestGridsCount = i + 1;
            }
        }

        if (bestGridsCount < maxGrids)
        {
            var gridsToRemove = maxGrids - bestGridsCount;

            for (int i = 0; i < gridsToRemove; i++)
            {
                RemoveGrid();
            }
        }

        this.Log(LogType.Info, $"Best fit size: {bestFitSize}, Grids[{bestGridsCount}]");

        return bestFitSize;
    }

    private float GetLettersFitSize(string[] words, int targetWordGrids)
    {
        var cellsY = words.Length - (words.Length / targetWordGrids) * (targetWordGrids - 1); //max words count
        var cellsX = words.Max(word => word.Length); //longest word letters count

        var gridUsefulSize = _wordGrids.First().LayoutGroup.GetUsefulSize(cellsY);

        var letterFitSize =
            (gridUsefulSize - Vector2.right * (cellsX - 1) * 10f) / new Vector2(cellsX, cellsY); // Todo: refactor

        var resultFitSize = letterFitSize.x > letterFitSize.y ? letterFitSize.y : letterFitSize.x;

        return resultFitSize;
    }

    private void AddWordsGrid()
    {
        var newGrid = Instantiate(wordGridPrefab, layoutGroup.transform);
        _wordGrids.Add(newGrid);
    }

    private void RemoveGrid()
    {
        Destroy(_wordGrids.First().gameObject);
        _wordGrids.RemoveAt(0);
    }

    private Vector2 GetWordsPanelUsefulSize() => layoutGroup.GetUsefulSize(3);

    public void ShowWord(string word, bool immediately = false)
    {
        _words[word].ShowWord(immediately).Forget();
    }
}