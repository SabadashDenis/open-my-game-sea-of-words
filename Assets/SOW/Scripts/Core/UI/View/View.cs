using System;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using SoW.Scripts.Core.UI.Animator;
using UnityEngine;

namespace SoW.Scripts.Core.UI
{
    public class View : SerializedMonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private RectTransform rectTransform;
        
        [TypeFilter("FilterAnimations")] [SerializeField] private IViewAnimator inAnimation;
        [TypeFilter("FilterAnimations")] [SerializeField] private IViewAnimator outAnimation;
        
        protected Tween animationTween;
        
        public CanvasGroup CG => canvasGroup;
        public RectTransform RT => rectTransform;
        
        public bool IsVisible => gameObject.activeSelf;
        
        [FoldoutGroup("API"), Button]
        public Tween Show()
        {
            animationTween?.Kill();
            if (inAnimation != null)
            {
                OnShowStart(false);
                gameObject.SetActive(true);
                animationTween = inAnimation.Animate(this)
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

            if (inAnimation != null)
            {
                gameObject.SetActive(true);
                inAnimation.AnimateImmediately(this);
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

            if (outAnimation != null)
            {
                OnHideStart(false);
                animationTween = outAnimation.Animate(this)
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

            if (outAnimation != null)
            {
                outAnimation.AnimateImmediately(this);
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
        
        
        protected IEnumerable<Type> FilterAnimations()
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