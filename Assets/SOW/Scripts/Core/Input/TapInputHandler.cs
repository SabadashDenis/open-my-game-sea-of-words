using System;
using SoW.Scripts.Core.Input._;

namespace SoW.Scripts.Core.Input
{
    public class TapInputHandler : InputHandlerBase
    {
        public event Action Released = delegate { };
        
        private bool _started;

        public bool Started => _started;
        
        protected override void InitInternal()
        {
        }

        protected override void OnTick()
        {
            if (!_started && UnityEngine.Input.GetMouseButtonDown(0))
            {
                _started = true;
            }

            if (_started && UnityEngine.Input.GetMouseButtonUp(0))
            {
                _started = false;
                Released.Invoke();
            }
        }
    }
}