using System.Threading;

namespace SoW.Scripts.Core.Scenario._
{
    public interface IAsyncScenario<TPreset> : IAsyncScenario
    {
        IAsyncScenario Play(in TPreset preset, CancellationToken token = default);
    }
    
    public interface IAsyncScenario
    {
        bool IsPlaying { get; }
        
        void Stop();
    }
}