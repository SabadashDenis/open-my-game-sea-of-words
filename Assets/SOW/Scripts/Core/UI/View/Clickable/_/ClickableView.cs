using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using SoW.Scripts.Core.UI.Animator;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SoW.Scripts.Core.UI
{
    public class ClickableView : View, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
    {
        [TypeFilter("FilterAnimations"), SerializeField] protected IViewAnimator downAnimation; 
        [TypeFilter("FilterAnimations"), SerializeField] protected IViewAnimator upAnimation;

        protected Tween pointerTween;
        
        public event Action OnClickEvent = delegate { };

        public void OnPointerDown(PointerEventData eventData)
        {
            pointerTween.Kill(true);

            if (downAnimation != null)
            {
                pointerTween = downAnimation.Animate(this);
            }
        }
        
        public void OnPointerUp(PointerEventData eventData)
        {
            pointerTween.Kill(true);
            
            if (upAnimation != null)
            {
                pointerTween = upAnimation.Animate(this);
            }
        }
        
        public void OnPointerClick(PointerEventData eventData) => OnClickEvent?.Invoke();
    }
}