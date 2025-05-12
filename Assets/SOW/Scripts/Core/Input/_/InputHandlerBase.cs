using SoW.Scripts.Core.Utility.Object.Initable;

namespace SoW.Scripts.Core.Input._
{
    public abstract class InputHandlerBase : InitableBehaviour, IInputHandler
    {
        private void Update()
        {
            if (IsInitialized)
                OnTick();
        }

        protected abstract void OnTick();
    }
}