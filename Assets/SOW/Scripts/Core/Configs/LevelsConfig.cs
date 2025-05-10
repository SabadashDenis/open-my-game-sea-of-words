using System;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using IOException = System.IO.IOException;

namespace SoW.Scripts.Core.Configs
{
    [Serializable]
    public class LevelsConfig : ConfigBase<LevelsConfigData>
    {
        [SerializeField] private LevelsConfigData levelsData;

        public override LevelsConfigData Data => levelsData;

#if UNITY_EDITOR
        [Button]
        private void ParseLevelDataFromJson()
        {
            string path = EditorUtility.OpenFilePanel("Select JSON File", "", "json");
            if (path.Length != 0)
            {
                try
                {
                    string json = File.ReadAllText(path);
                    var levelData = JsonUtility.FromJson<LevelData>(json);
                    levelsData.data.Add(levelData);
                    this.Log(LogType.Info, "Words loaded successfully.");
                }
                catch (IOException e)
                {
                    this.Log(LogType.Error, $"Failed to load file: {e.Message}");
                }
                catch (Exception e)
                {
                    this.Log(LogType.Error, $"Failed to parse JSON: {e.Message}");
                }
            }
        }
#endif
    }

    [Serializable]
    public struct LevelsConfigData
    {
        public List<LevelData> data;
    }

    [Serializable]
    public struct LevelData
    {
        public string[] words;
    }
}