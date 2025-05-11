namespace SoW.Scripts.Core.Save._
{
    public interface ISaver
    {
        T Get<T>() where T : class;
        void Reset<T>() where T : class;
        void Save<T>() where T : class;
        void SaveAll();
    }
}