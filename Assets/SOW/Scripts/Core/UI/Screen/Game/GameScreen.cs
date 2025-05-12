using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using SoW.Scripts.Core;
using SoW.Scripts.Core.UI;
using SoW.Scripts.Core.UI.Screen.Game.Views;
using SoW.Scripts.Core.Utility.Extended;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameScreen : ScreenViewBase
{
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private HorizontalLayoutGroup layoutGroup;
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

        for (int i = 0; i < wordPacks.Count; i++)
        {
            var targetWordsGrid = _wordGrids[i];

            foreach (var word in wordPacks[i])
            {
                var wordLine = targetWordsGrid.AddWord(word);
                _words.Add(word, wordLine);
            }
        }

        foreach (var wordLine in _words.Values)
        {
            foreach (var letterView in wordLine.Letters)
            {
                letterView.SetSize(letterSize);
            }
        }
    }

    public void Clear()
    {
        while (_wordGrids.Count > 0)
        {
            RemoveGrid(_wordGrids[0]);
        }

        _words.Clear();
    }

    public float GetCurrentLettersFitSize(string[] words)
    {
        var currentGridsCount = _wordGrids.Count;

        if (currentGridsCount > 0)
        {
            var cellsY = words.Length - (words.Length / currentGridsCount) * (currentGridsCount - 1); //max words count
            var cellsX = words.Max(word => word.Length); //longest word letters count

            var grid = _wordGrids.First();
            var gridUsefulSize = grid.LayoutGroup.GetUsefulSize(grid.RT, cellsY);

            var letterFitSize =
                (gridUsefulSize - Vector2.right * (cellsX - 1) * 10f) / new Vector2(cellsX, cellsY); // Todo: refactor

            var resultFitSize = letterFitSize.x > letterFitSize.y ? letterFitSize.y : letterFitSize.x;

            return resultFitSize;
        }

        return 0;
    }

    public WordsGrid AddWordsGrid()
    {
        var newGrid = SoWPool.I.WordGridsPool.Pop<WordsGrid>(layoutGroup.transform);
        _wordGrids.Add(newGrid);
        
        return newGrid;
    }

    public void RemoveGrid(WordsGrid grid)
    {
        grid.Clear();
        _wordGrids.Remove(grid);
        SoWPool.I.WordGridsPool.Push(grid);
    }

    public void ShowWord(string word, CancellationToken token, bool immediately = false)
    {
        _words[word].ShowWord(token, immediately).Forget();
    }
}