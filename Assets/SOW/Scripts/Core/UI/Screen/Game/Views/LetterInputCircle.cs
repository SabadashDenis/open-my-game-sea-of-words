using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SoW.Scripts.Core.UI.Screen.Game.Views
{
    public class LetterInputCircle : View
    {
        [SerializeField] private LetterView letterViewPrefab;
        [SerializeField] private Transform lettersRoot;
        [SerializeField] private float offsetFromCenter;
        
        private List<LetterView> _letterViews = new();
        
        [FoldoutGroup("API"), Button]
        public void SetupLetters(string lettersStr)
        {
            ClearLetters();
            
            int letterCount = lettersStr.Length;
            float angleStep = 360f / letterCount;

            for (int i = 0; i < letterCount; i++)
            {
                var letterView = AddLetter(lettersStr[i]);
                
                float angle = i * angleStep * Mathf.Deg2Rad;
                Vector2 position = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * offsetFromCenter;
                
                letterView.transform.localPosition = position;
            }
        }

        private LetterView AddLetter(char letter)
        {
            var letterView = Instantiate(letterViewPrefab, lettersRoot);
            letterView.SetLetter(letter);
            _letterViews.Add(letterView);
            
            return letterView;
        }

        [FoldoutGroup("API"), Button]
        private void ClearLetters()
        {
            foreach (var letterView in _letterViews)
            {
                Destroy(letterView.gameObject);
            }
            
            _letterViews.Clear();
        }
    }
}