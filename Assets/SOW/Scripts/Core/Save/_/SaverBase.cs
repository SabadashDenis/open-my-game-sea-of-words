namespace SoW.Scripts.Core.Save._
{
    public abstract class SaverBase : ISaver
    {
        public abstract T Get<T>() where T : class;

        public abstract void Reset<T>() where T : class;

        public abstract void Save<T>() where T : class;

        public abstract void SaveAll();
    }
}