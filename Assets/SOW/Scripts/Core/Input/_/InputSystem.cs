using UnityEngine;

namespace SoW.Scripts.Core.Input._
{
    public class InputSystem : SystemBase
    {
        [SerializeField] private TapInputHandler tapHandler;
        
        public TapInputHandler Tap => tapHandler;
        
        protected override void InitInternal(SystemData data)
        {
            tapHandler.Init();
        }
    }
}