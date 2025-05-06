using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using SoW.Scripts.Core.UI.Screen.Game.Views.LetterView;
using UnityEngine;
using UnityEngine.Serialization;

namespace SoW.Scripts.Core.UI.Screen.Game.Views
{
    public class LetterInputCircle : View
    {
        [FormerlySerializedAs("letterViewPrefab")] [SerializeField] private LetterViewBase letterViewBasePrefab;
        [SerializeField] private Transform lettersRoot;
        [SerializeField] private float offsetFromCenter;
        
        private List<LetterViewBase> _letterViews = new();
        
        public event Action<char, bool> OnInputChanged = delegate { };
        
        [FoldoutGroup("API"), Button]
        public void SetupLetters(string lettersStr)
        {
            ClearLetters();
            
            int letterCount = lettersStr.Length;
            float angleStep = 360f / letterCount;

            for (int i = 0; i < letterCount; i++)
            {
                var letterView = AddLetter(lettersStr[i]);
                letterView.OnSelectionChanged += OnInputChanged;
                
                float angle = i * angleStep * Mathf.Deg2Rad;
                Vector2 position = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * offsetFromCenter;
                
                letterView.transform.localPosition = position;
            }
        }

        private LetterViewBase AddLetter(char letter)
        {
            var letterView = Instantiate(letterViewBasePrefab, lettersRoot);
            letterView.SetLetter(letter);
            _letterViews.Add(letterView);
            
            return letterView;
        }

        [FoldoutGroup("API"), Button]
        private void ClearLetters()
        {
            foreach (var letterView in _letterViews)
            {
                letterView.OnSelectionChanged -= OnInputChanged;
                
                Destroy(letterView.gameObject);
            }
            
            _letterViews.Clear();
        }
    }
}