using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using SoW.Scripts.Core.Factory._;
using SoW.Scripts.Core.UI;
using SoW.Scripts.Core.UI.Screen.Game.Views;
using UnityEngine;

public class WordsGridLine : View
{
    [SerializeField] private Transform letterRoot;
    [SerializeField] private float showLetterDelay;

    private List<LetterView> _letters = new();
    
    public void SetupWord(string word)
    {
        Clear();
        
        foreach (var letter in word)
        {
            AddLetter(letter);
        }
    }

    public async UniTask ShowWord()
    {
        foreach (var letterView in _letters)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(showLetterDelay), cancellationToken: this.GetCancellationTokenOnDestroy());
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
        letterView.SetColorScheme(LetterColorSchemeType.Normal);
        
        _letters.Add(letterView);
    }
}
