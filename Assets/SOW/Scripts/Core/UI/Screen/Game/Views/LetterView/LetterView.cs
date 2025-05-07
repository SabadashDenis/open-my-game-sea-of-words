using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using SoW.Scripts.Core.UI.Screen.Game.Views._;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SoW.Scripts.Core.UI.Screen.Game.Views.LetterView
{
    public class LetterView : View, ILetterView
    {
        [SerializeField] private Dictionary<LetterColorSchemeType, LetterColorScheme> colorConfig = new();
        [SerializeField] private TMP_Text letterText;
        [SerializeField] private Image letterBg;
        
        public event Action<char, bool> OnSelectionChanged = delegate { };
        
        private char _currentLetter;

        public bool IsSelected { get; private set; }
        
        public void SetLetter(char letter)
        {
            _currentLetter = letter;
            letterText.text = letter.ToString();
        }

        [FoldoutGroup("API"), Button]
        public void SetSize(int pixelSize)
        {
            RT.sizeDelta = Vector2.one * pixelSize;
        }

        [FoldoutGroup("API"), Button]
        public void SetColorScheme(LetterColorSchemeType colorSchemeType)
        {
            var targetScheme = colorConfig[colorSchemeType];

            letterText.color = targetScheme.LetterColor;
            letterBg.color = targetScheme.BgColor;
        }

        protected void SetSelected(bool condition)
        {
            var colorScheme = condition ? LetterColorSchemeType.Selected : LetterColorSchemeType.Normal;
            SetColorScheme(colorScheme);

            IsSelected = condition;
            OnSelectionChanged.Invoke(_currentLetter, condition);
        }

        public void ClearSelection() => SetSelected(false);
    }

    public enum LetterColorSchemeType
    {
        Normal,
        Selected
    }

    public struct LetterColorScheme
    {
        public Color LetterColor;
        public Color BgColor;
    }
}