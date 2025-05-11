using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using SoW.Scripts.Core;
using SoW.Scripts.Core.UI;
using SoW.Scripts.Core.UI.Screen.Game.Views;
using UnityEngine;

public class WordsGridLine : View
{
    [SerializeField] private Transform letterRoot;
    [SerializeField] private int letterSize;
    [SerializeField] private float showLetterDelay;

    private List<LetterView> _letters = new();

    public List<LetterView> Letters => _letters;

    public void SetupWord(string word)
    {
        Clear();

        foreach (var letter in word)
        {
            AddLetter(letter);
        }
    }

    public async UniTask ShowWord(bool immediately = false)
    {
        var token = this.GetCancellationTokenOnDestroy();
        
        foreach (var letterView in _letters)
        {
            if (!immediately)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(showLetterDelay), cancellationToken: token);
            }

            letterView.SetColorScheme(LetterColorSchemeType.Selected);
        }
    }

    public void Clear()
    {
        foreach (var letterView in _letters)
        {
            SoWPool.I.LettersPool.Push(letterView);
        }

        _letters.Clear();
    }

    private void AddLetter(char letter)
    {
        var letterView = SoWPool.I.LettersPool.Pop<LetterView>(letterRoot);
        letterView.SetLetter(letter);
        letterView.SetSize(letterSize);
        letterView.SetColorScheme(LetterColorSchemeType.Normal);

        _letters.Add(letterView);
    }
}