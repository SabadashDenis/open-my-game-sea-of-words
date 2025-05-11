using TMPro;
using UnityEngine;

namespace SoW.Scripts.Core.UI.Clickable
{
    public class TextClickableView : ClickableView
    {
        [SerializeField] private TMP_Text btnText;

        public void SetText(string text) => btnText.text = text;
    }
}