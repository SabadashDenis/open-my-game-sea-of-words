using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SoW.Scripts.Core.UI.Screen.Game.Views
{
    public class LetterInputCircle : View 
    {
        [SerializeField] private Transform lettersRoot;
        [SerializeField] private float offsetFromCenter;
        
        private List<SelectableLetterView> _letterViews = new();
        
        public IReadOnlyList<SelectableLetterView> LetterViews => _letterViews;
        
        [FoldoutGroup("API"), Button]
        public void SetupLetters(string lettersStr)
        {
            ClearLetters();
            
            int letterCount = lettersStr.Length;
            float angleStep = 360f / letterCount;

            for (int i = 0; i < letterCount; i++)
            {
                var letterView = SoWPool.I.LettersPool.Pop<SelectableLetterView>(lettersRoot);
                letterView.SetLetter(lettersStr[i]);
                _letterViews.Add(letterView);
                
                float angle = i * angleStep * Mathf.Deg2Rad;
                Vector2 position = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * offsetFromCenter;
                
                letterView.transform.localPosition = position;
            }
        }

        public void ClearSelection()
        {
            foreach (var letterView in _letterViews)
            {
                letterView.SetSelected(false, false);
            }
        }

        [FoldoutGroup("API"), Button]
        private void ClearLetters()
        {
            foreach (var letterView in _letterViews)
            {
                SoWPool.I.LettersPool.Push(letterView);
            }
            
            _letterViews.Clear();
        }
    }
}