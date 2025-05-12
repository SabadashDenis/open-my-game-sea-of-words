using System;
using DG.Tweening;
using UnityEngine;

namespace SoW.Scripts.Core.UI.Animator
{
    [Serializable]
    public class Fade : ViewAnimatorBase
    {
        [SerializeField] private float delay;
        [SerializeField] private float duration;
        [SerializeField] private float opacity;
        [SerializeField] private Ease ease;
        
        public override Tween Animate(View view)
        {
            var tween = view.CG.DOFade(opacity, duration)
                .SetDelay(delay)
                .SetEase(ease)
                .SetUpdate(true);
            return tween;
        }
        
        public override void AnimateImmediately(View view)
        {
            view.CG.alpha = opacity;
        }
    }
}