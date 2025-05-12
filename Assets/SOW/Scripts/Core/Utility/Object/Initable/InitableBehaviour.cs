using UnityEngine;

namespace SoW.Scripts.Core.Utility.Object.Initable
{
    public abstract class InitableBehaviour : MonoBehaviour, IInitable
    {
        public bool IsInitialized { get; private set; }
        
        public void Init()
        {
            if (!IsInitialized)
            {
                IsInitialized = true;
                InitInternal();
            }
            else
            {
                this.Log(LogType.Warning, "Already Initialized");
            }
        }
        
        protected abstract void InitInternal();
    }

    public abstract class InitableBehaviour<TData> : MonoBehaviour, IInitable<TData>
    {
        protected TData Data { get; private set; }
        public bool IsInitialized { get; private set; }
        
        public void Init(TData data)
        {
            if (!IsInitialized)
            {
                IsInitialized = true;
                
                Data = data;
                InitInternal(data);
            }
            else
            {
                this.Log(LogType.Warning, "Already Initialized");
            }
        }
        
        protected abstract void InitInternal(TData data);
    }
}