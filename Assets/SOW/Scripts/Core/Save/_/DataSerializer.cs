using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace SoW.Scripts.Core.Save._
{
    public static class DataSerializer
    {
        private static BinaryFormatter _formatter = new ();
		
        public static string Serialize<T>(T obj)
        {
            var data = "";

            if (obj.GetType().IsSerializable)
            {
                using var stream = new MemoryStream();
				
                _formatter.Serialize(stream, obj);
                data = Convert.ToBase64String(stream.ToArray());
            }
            else
            {
                Debug.LogError($"[DataSerializer] {obj.GetType()} is not serializable.]");
            }

            return data;
        }

        public static T Deserialize<T>(string serializedData)
        {
            if (serializedData.Length != 0)
            {
                var bytes = Convert.FromBase64String(serializedData);

                using var stream = new MemoryStream(bytes);
				
                return (T) _formatter.Deserialize(stream);
            }
            
            return default;
        }
    }
}