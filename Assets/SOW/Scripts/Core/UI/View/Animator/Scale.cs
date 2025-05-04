using System;
using DG.Tweening;
using UnityEngine;

namespace SoW.Scripts.Core.UI.Animator
{
    [Serializable]
    public class Scale : ViewAnimatorBase
    {
        [SerializeField] private float delay;
        [SerializeField] private float duration;
        [SerializeField] private Vector3 scale;
        [SerializeField] private Ease ease;
        
        public override Tween Animate(View view)
        {
            var tween = view.RT.DOScale(scale, duration)
                .SetDelay(delay)
                .SetEase(ease)
                .SetUpdate(true);
            return tween;
        }
        
        public override void AnimateImmediately(View view)
        {
            view.RT.localScale = scale;
        }
    }
}