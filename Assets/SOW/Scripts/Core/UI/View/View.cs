using System;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using SoW.Scripts.Core.UI.Animator;
using SoW.Scripts.Core.Utility.Object.Initable;
using UnityEngine;

namespace SoW.Scripts.Core.UI
{
    public class View : SerializedMonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private RectTransform rectTransform;
        
        [TypeFilter("FilterAnimations")] [SerializeField] private IViewAnimator InAnimation;
        [TypeFilter("FilterAnimations")] [SerializeField] private IViewAnimator OutAnimation;
        
        protected Tween animationTween;
        
        public CanvasGroup CG => canvasGroup;
        public RectTransform RT => rectTransform;
        
        public bool IsVisible => gameObject.activeSelf;
        
        [FoldoutGroup("API"), Button]
        public Tween Show()
        {
            animationTween?.Kill();
            if (InAnimation != null)
            {
                OnShowStart(false);
                gameObject.SetActive(true);
                animationTween = InAnimation.Animate(this)
                    .OnComplete(() =>
                    {
                        OnShowEnd(false);
                    });
            }
            else
            {
                OnShowStart(false);
                gameObject.SetActive(true);
                OnShowEnd(false);
            }

            return animationTween;
        }
            
        [FoldoutGroup("API"), Button]
        public void ShowImmediately()
        {
            animationTween?.Kill();
            
            OnShowStart(true);

            if (InAnimation != null)
            {
                gameObject.SetActive(true);
                InAnimation.AnimateImmediately(this);
            }
            else
            {
                gameObject.SetActive(true);
            }
            
            OnShowStart(false);
        }
        
        [FoldoutGroup("API"), Button]
        public Tween Hide()
        {
            animationTween?.Kill(true);

            if (OutAnimation != null)
            {
                OnHideStart(false);
                animationTween = OutAnimation.Animate(this)
                    .OnComplete(() =>
                    {
                        gameObject.SetActive(false);
                        OnHideEnd(false);
                    });
            }
            else
            {
                OnHideStart(false);
                gameObject.SetActive(false);
                OnHideEnd(false);
            }

            return animationTween;
        }

        [FoldoutGroup("API"), Button]
        public void HideImmediately()
        {
            animationTween?.Kill();
            
            OnHideStart(true);

            if (OutAnimation != null)
            {
                OutAnimation.AnimateImmediately(this);
                gameObject.SetActive(false);
            }
            else
            {
                gameObject.SetActive(false);
            }

            OnHideEnd(false);
        }
        
        protected virtual void OnShowStart(bool immediately) { }
        protected virtual void OnShowEnd(bool immediately) { }
        
        protected virtual void OnHideStart(bool immediately) { }
        protected virtual void OnHideEnd(bool immediately) { }
        
        
        private IEnumerable<Type> FilterAnimations()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            List<Type> result = new List<Type>();

            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes();

                foreach (var type in types)
                {
                    if (typeof(IViewAnimator).IsAssignableFrom(type) && !type.IsAbstract && !type.IsInterface)
                    {
                        result.Add(type);
                    }
                }
            }

            return result;
        }
    }
}