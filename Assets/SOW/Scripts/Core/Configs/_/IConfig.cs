namespace SoW.Scripts.Core.Configs._
{
    public interface IConfig<TConfigData>
    {
        TConfigData Data { get; }
    }
}