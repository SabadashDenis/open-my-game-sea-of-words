using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using SoW.Scripts.Core.Configs._;
using SoW.Scripts.Core.Input._;
using SoW.Scripts.Core.Utility.Object.Initable;

namespace SoW.Scripts.Core.Scenario._
{
    public abstract class AsyncScenarioBase<TPreset> : AsyncScenarioBase, IAsyncScenario<TPreset>
    {
        protected TPreset Preset { get; private set; }

        public IAsyncScenario Play(in TPreset preset, CancellationToken token = default)
        {
            if (!IsPlaying)
                Preset = preset;

            return Play();
        }
    }

    public abstract class AsyncScenarioBase : InitableBehaviour<ScenarioData>, IAsyncScenario
    {
        private CancellationTokenSource _cancellation;

        protected CancellationToken Token => _cancellation?.Token ?? CancellationToken.None;

        public bool IsPlaying { get; private set; }

        public IAsyncScenario Play(CancellationToken token = default)
        {
            if (IsPlaying)
                return this;

            _cancellation?.Cancel();
            _cancellation = CancellationTokenSource.CreateLinkedTokenSource(token);

            IsPlaying = true;

            AsyncPlay(_cancellation.Token).Forget();

            return this;
        }

        public void Stop()
        {
            if (!IsPlaying)
                return;

            _cancellation?.Cancel();

            StopInternal();
            IsPlaying = false;
        }
        
        protected abstract void StopInternal();
        protected abstract UniTask AsyncPlayInternal(CancellationToken token);

        private async UniTask AsyncPlay(CancellationToken token)
        {
            try
            {
                await AsyncPlayInternal(token);
            }
            catch (Exception e) when (e is not OperationCanceledException)
            {
                this.Log(LogType.Error, $"Exception: {e.Message}");
                ;
            }

            if (!token.IsCancellationRequested)
                Stop();
        }
    }
    
    public struct ScenarioData
    {
        public readonly UISystem UI;
        public readonly ScenarioSystem Scenario;
        public readonly InputSystem Input;
        public readonly ConfigSystem Config;
        
        public ScenarioData(UISystem ui, ScenarioSystem scenario, InputSystem input, ConfigSystem config)
        {
            UI = ui;
            Scenario = scenario;
            Input = input;
            Config = config;
        }
    }
}