using System.Collections.Generic;
using UnityEngine;

namespace SoW.Scripts.Core.UI.Screen.Game.Views
{
    public class LetterInputResult : View
    {
        [SerializeField] private float letterSize;
        [SerializeField] private Transform letterRoot;
        
        private Stack<LetterView> _letterViews = new();

        public void Append(char letter)
        {
            var letterView = SoWPool.I.LettersPool.Pop<LetterView>(letterRoot);
            letterView.SetSize(letterSize);
            letterView.SetColorScheme(LetterColorSchemeType.Visible);
            letterView.SetLetter(letter);
            _letterViews.Push(letterView);
        }

        public void Clear()
        {
            var lettersCount = _letterViews.Count;
            
            for (int i = 0; i < lettersCount; i++)
            {
                RemoveLast();
            }
        }
        
        public void RemoveLast()
        {
            var lastLetter = _letterViews.Pop();
            SoWPool.I.LettersPool.Push(lastLetter);
        }
    }
}