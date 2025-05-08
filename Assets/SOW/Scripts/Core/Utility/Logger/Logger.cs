using UnityEngine;
using Object = System.Object;

namespace SoW.Scripts.Core
{
    public static class Logger
    {
        public static void Log(this Object obj, LogType type, string message)
        {
            var logText = $"[{obj.GetType().Name}] {type.ToString()}: {message}";

            switch (type)
            {
                case LogType.Info : Debug.Log(logText); break;
                case LogType.Warning: Debug.LogWarning(logText); break;
                case LogType.Error : Debug.LogError(logText); break;
            }
        }
    }
    
    public enum LogType
    {
        Info = 0,
        Warning = 1,
        Error = 2
    }
}