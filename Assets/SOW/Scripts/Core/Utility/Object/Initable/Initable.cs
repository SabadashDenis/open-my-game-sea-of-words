namespace SoW.Scripts.Core.Utility.Object.Initable
{
    public abstract class Initable : IInitable
    {
        public bool IsInitialized { get; private set; }
        
        public void Init()
        {
            if (!IsInitialized)
            {
                IsInitialized = true;
            }
            else
            {
                this.Log(LogType.Warning, "Already Initialized");
            }
        }
        
        protected abstract void InitInternal();
    }
    
    public abstract class Initable<TData> : IInitable<TData>
    {
        protected TData Data { get; private set; }
        public bool IsInitialized { get; private set; }
        
        public void Init(TData data)
        {
            if (!IsInitialized)
            {
                Data = data;
                IsInitialized = true;
            }
            else
            {
                this.Log(LogType.Warning, "Already Initialized");
            }
        }
        
        protected abstract void InitInternal(TData data);
    }
}