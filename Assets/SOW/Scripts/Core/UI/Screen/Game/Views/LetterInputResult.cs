using System.Collections.Generic;
using SoW.Scripts.Core.UI.Screen.Game.Views.LetterView;
using UnityEngine;

namespace SoW.Scripts.Core.UI.Screen.Game.Views
{
    public class LetterInputResult : View
    {
        [SerializeField] private LetterView.LetterView letterPrefab;
        [SerializeField] private Transform letterRoot;
        
        private Stack<LetterView.LetterView> _letterViews = new();

        public void Append(char letter)
        {
            var newLetterView = Instantiate(letterPrefab, letterRoot);
            newLetterView.SetSize(70);
            newLetterView.SetLetter(letter);
            _letterViews.Push(newLetterView);
        }

        public void RemoveLast()
        {
            var lastLetter = _letterViews.Pop();
            Destroy(lastLetter.gameObject);
        }
    }
}