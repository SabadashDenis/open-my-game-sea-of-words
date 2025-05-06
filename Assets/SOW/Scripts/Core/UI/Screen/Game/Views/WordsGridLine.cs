using System.Collections.Generic;
using SoW.Scripts.Core.UI;
using SoW.Scripts.Core.UI.Screen.Game.Views.LetterView;
using UnityEngine;

public class WordsGridLine : View
{
    [SerializeField] private LetterViewBase letterPrefab;
    [SerializeField] private Transform letterRoot;

    private List<LetterViewBase> _letters = new();
    
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
            Destroy(letterView.gameObject);
        }
        
        _letters.Clear();
    }

    private void AddLetter(char letter)
    {
        var newLetter = Instantiate(letterPrefab, letterRoot);
        newLetter.SetLetter(letter);
        
        _letters.Add(newLetter);
    }
}
