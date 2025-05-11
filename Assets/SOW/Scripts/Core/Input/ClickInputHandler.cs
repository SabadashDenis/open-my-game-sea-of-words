using System;
using SoW.Scripts.Core.Input._;

namespace SoW.Scripts.Core.Input
{
    public class ClickInputHandler : InputHandlerBase
    {
        public event Action Released = delegate { };
        
        public bool Started { get; private set; }

        protected override void InitInternal()
        {
        }

        protected override void OnTick()
        {
            if (!Started && IsTapStarted())
            {
                Started = true;
            }

            if (Started && IsTapEnded())
            {
                Started = false;
                Released.Invoke();
            }
        }

        private bool IsTapStarted()
        {
#if !UNITY_EDITOR && UNITY_ANDROID
            return UnityEngine.Input.touches.Any(touch => touch.phase == TouchPhase.Began);
#endif
            return UnityEngine.Input.GetMouseButtonDown(0);
        }

        private bool IsTapEnded()
        {
#if !UNITY_EDITOR && UNITY_ANDROID
            return UnityEngine.Input.touches.Any(touch => touch.phase == TouchPhase.Ended);
#endif
            return UnityEngine.Input.GetMouseButtonUp(0);
        }
    }
}