using System.Threading;
using Cysharp.Threading.Tasks;
using SoW.Scripts.Core.Scenario._;

namespace SoW.Scripts.Core.Utility.Extended
{
    public static class ScenarioExtensions
    {
        public static async UniTask WaitForEnd(this IAsyncScenario scenario, CancellationToken token = default)
        {
            await UniTask.WaitWhile(() => scenario.IsPlaying, cancellationToken: token);
        }
    }
}