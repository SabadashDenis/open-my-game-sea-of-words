using SoW.Scripts.Core.Configs._;

namespace SoW.Scripts.Core.Configs
{
    public abstract class ConfigBase<TConfigData> : IConfig<TConfigData>
    {
        public abstract TConfigData Data { get;}
    }
}