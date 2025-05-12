using DG.Tweening;

namespace SoW.Scripts.Core.UI.Animator
{
    public interface IViewAnimator
    {
        Tween Animate(View view);
        void AnimateImmediately(View view);
    }
}