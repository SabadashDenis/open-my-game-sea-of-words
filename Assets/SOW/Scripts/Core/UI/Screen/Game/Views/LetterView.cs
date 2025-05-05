using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace SoW.Scripts.Core.UI.Screen.Game.Views
{
    public class LetterView : View
    {
        [SerializeField] private LetterColorConfig colorConfig;
        
        [SerializeField] private TMP_Text letterText;
        [SerializeField] private Image letterBg;
        
        public void SetLetter(char letter) => letterText.text = letter.ToString();

        [FoldoutGroup("API"), Button]
        public void SetSize(int pixelSize)
        {
            RT.sizeDelta = Vector2.one * pixelSize;
        }
        
        [FoldoutGroup("API"), Button]
        public void SetSelected(bool condition)
        {
            letterText.color = condition ? colorConfig.LetterSelectedColor : colorConfig.LetterNormalColor;
            letterBg.color = condition ? colorConfig.BgSelectedColor : colorConfig.BgNormalColor;
        }
    }

    [Serializable]
    public struct LetterColorConfig
    {
        public Color LetterNormalColor;
        public Color LetterSelectedColor;
        
        public Color BgNormalColor;
        public Color BgSelectedColor;
    }
}