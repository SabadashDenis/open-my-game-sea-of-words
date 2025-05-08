using System.Collections.Generic;
using SoW.Scripts.Core.Factory._;
using UnityEngine;

namespace SoW.Scripts.Core.UI.Screen.Game.Views
{
    public class LetterInputResult : View
    {
        [SerializeField] private Transform letterRoot;
        
        private Stack<LetterView> _letterViews = new();

        public void Append(char letter)
        {
            var letterView = SoWPool.I.LettersPool.Pop<LetterView>(letterRoot);
            letterView.SetSize(70);
            letterView.SetLetter(letter);
            _letterViews.Push(letterView);
        }

        public void RemoveLast()
        {
            var lastLetter = _letterViews.Pop();
            SoWPool.I.LettersPool.Push(lastLetter);
        }
    }
}