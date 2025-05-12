using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using SoW.Scripts.Core.UI.Screen.Game.Views._;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SoW.Scripts.Core.UI.Screen.Game.Views
{
    public class LetterView : View, ILetterView
    {
        [SerializeField] private Dictionary<LetterColorSchemeType, LetterColorScheme> colorConfig = new();
        [SerializeField] private TMP_Text letterText;
        [SerializeField] private Image letterBg;
        
        private char _currentLetter;
        public char CurrentLetter => _currentLetter;
        
        public void SetLetter(char letter)
        {
            _currentLetter = letter;
            letterText.text = letter.ToString();
        }

        [FoldoutGroup("API"), Button]
        public void SetSize(float pixelSize)
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
    }

    public enum LetterColorSchemeType
    {
        Visible,
        Selected,
        Hidden
    }

    public struct LetterColorScheme
    {
        public Color LetterColor;
        public Color BgColor;
    }
}