namespace SoW.Scripts.Core.Utility.Object.Initable
{
    public interface IInitable
    {
        bool IsInitialized { get; }
        void Init();
    }

    public interface IInitable<TData>
    {
        bool IsInitialized { get; }
        void Init(TData data);
    }
}