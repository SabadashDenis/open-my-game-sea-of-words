using SoW.Scripts.Core.Configs._;
using SoW.Scripts.Core.Input._;
using SoW.Scripts.Core.Save._;
using SoW.Scripts.Core.Utility.Object.Initable;

namespace SoW.Scripts.Core.Scenario._
{
    public abstract class ScenarioBase : InitableBehaviour<ScenarioData>
    {
        public bool IsPlaying { get; private set; }
        
        public void Play()
        {
            if (!IsInitialized)
            {
                this.Log(LogType.Error, "Scenario not initialized");
                return;
            }

            if (IsPlaying)
            {
                this.Log(LogType.Error, "Try play already playing scenario");
                return;
            }

            IsPlaying = true;
            PlayInternal();
        }

        private void Update()
        {
            if (IsInitialized && IsPlaying)
            {
                OnTick();
            }
        }

        public void Stop()
        {
            if (!IsInitialized)
            {
                this.Log(LogType.Error, "Scenario not initialized");
                return;
            }

            if (!IsPlaying)
            {
                this.Log(LogType.Error, "Try stop not playing scenario");
                return;
            }
            
            IsPlaying = false;
            StopInternal();
        }

        protected abstract void PlayInternal();

        protected abstract void OnTick();
        protected abstract void StopInternal();
        
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