using DG.Tweening;

namespace SoW.Scripts.Core.UI.Animator
{
    public abstract class ViewAnimatorBase : IViewAnimator
    {
        public abstract Tween Animate(View view);

        public abstract void AnimateImmediately(View view);
    }
}