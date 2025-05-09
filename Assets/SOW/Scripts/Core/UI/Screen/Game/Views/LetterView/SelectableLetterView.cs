using System;
using UnityEngine.EventSystems;

namespace SoW.Scripts.Core.UI.Screen.Game.Views
{
    public class SelectableLetterView : LetterView, IPointerEnterHandler, IPointerDownHandler
    {
        public event Action<SelectableLetterView> Hover = delegate { };
        public event Action<SelectableLetterView> Tap = delegate { };
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            Hover.Invoke(this);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Tap.Invoke(this);
        }
    }
}