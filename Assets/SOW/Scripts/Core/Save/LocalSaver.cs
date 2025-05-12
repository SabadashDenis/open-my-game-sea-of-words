using System;
using System.Collections.Generic;
using SoW.Scripts.Core.Save._;
using UnityEngine;

namespace SoW.Scripts.Core.Save
{
    public class LocalSaver : SaverBase
    {
        private readonly Dictionary<Type, object> _datas = new ();
        
         public override T Get<T>() where T : class
        {
            if (_datas.TryGetValue(typeof(T), out var data))
            {
                return data as T;
            }

            if (PlayerPrefs.HasKey(typeof(T).FullName))
            {
                var prefsDataStr = PlayerPrefs.GetString(typeof(T).FullName);
                var prefsDataObj = DataSerializer.Deserialize<T>(prefsDataStr);
                
                _datas.Add(typeof(T), prefsDataObj);
                return prefsDataObj;
            }

            var newData = Activator.CreateInstance<T>();
            _datas.Add(typeof(T), newData);
            
            return newData;
        }

        public override void Reset<T>() where T : class
        {
            if (PlayerPrefs.HasKey(typeof(T).FullName))
            {
                var prefsDataStr = PlayerPrefs.GetString(typeof(T).FullName);
                var prefsDataObj = DataSerializer.Deserialize<T>(prefsDataStr);
                
                _datas[typeof(T)] = prefsDataObj;
                return;
            }
            var newData = Activator.CreateInstance<T>();
            _datas[typeof(T)] = newData;
        }

        public override void Save<T>() where T : class
        {
            if (_datas.TryGetValue(typeof(T), out var data))
            {
                var prefsDataStr = DataSerializer.Serialize(data);
                
                PlayerPrefs.SetString(typeof(T).FullName, prefsDataStr);
                return;
            }
            
            var newData = Activator.CreateInstance<T>();
            var prefsNewDataStr = DataSerializer.Serialize(newData);
            
            PlayerPrefs.SetString(typeof(T).FullName, prefsNewDataStr);
        }

        public override void SaveAll()
        {
            foreach (var pair in _datas)
            {
                var prefsDataStr = DataSerializer.Serialize(pair.Value);
                
                PlayerPrefs.SetString(pair.Key.FullName, prefsDataStr);
            }
            
            PlayerPrefs.Save();
        }
    }
}