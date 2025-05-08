using System.Collections.Generic;
using SoW.Scripts.Core.Factory._;
using SoW.Scripts.Core.UI;
using SoW.Scripts.Core.UI.Screen.Game.Views;
using UnityEngine;

public class WordsGridLine : View
{
    [SerializeField] private Transform letterRoot;

    private List<LetterView> _letters = new();
    
    public void SetupWord(string word)
    {
        Clear();
        
        foreach (var letter in word)
        {
            AddLetter(letter);
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
        
        _letters.Add(letterView);
    }
}
