using UnityEngine;

namespace SoW.Scripts.Core.Input._
{
    public class InputSystem : SystemBase
    {
        [SerializeField] private ClickInputHandler click;
        
        public ClickInputHandler Click => click;
        
        protected override void InitInternal(SystemData data)
        {
            click.Init();
        }
    }
}