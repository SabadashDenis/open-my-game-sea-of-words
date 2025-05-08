using UnityEngine;
using UnityEngine.EventSystems;

namespace SoW.Scripts.Core.UI.Screen.Game.Views
{
    public class SelectableLetterView : LetterView, IPointerEnterHandler, IPointerDownHandler
    {
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (Input.GetMouseButton(0)) //TODO:Refactor
                SetSelected(!IsSelected);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            SetSelected(!IsSelected);
        }
    }
}