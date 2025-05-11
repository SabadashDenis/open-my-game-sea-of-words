using SoW.Scripts.Core.UI.Clickable;
using TMPro;
using UnityEngine;

namespace SoW.Scripts.Core.UI.Screen.Win
{
    public class WinScreen : ScreenViewBase
    {
        [SerializeField] private TMP_Text lvlPassText;
        [SerializeField] private TMP_Text lvlPassAdditionalText;
        [SerializeField] private TextClickableView nextLvlBtn;
        
        public ClickableView NextLvlBtn => nextLvlBtn;

        public void SetupTexts(int passedLevelNumber)
        {
            lvlPassText.text = $"Уровень {passedLevelNumber} пройден";
            lvlPassAdditionalText.text = $"Изумительно!";
            
            nextLvlBtn.SetText($"Уровень {passedLevelNumber + 1}");
        }
    }
}