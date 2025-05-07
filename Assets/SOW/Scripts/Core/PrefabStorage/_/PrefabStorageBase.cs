using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SoW.Scripts.Core.PrefabStorage._
{
    public abstract class PrefabStorageBase<TPrefab> : IPrefabStorage<TPrefab>
        where TPrefab : MonoBehaviour
    {
        [SerializeField] private List<TPrefab> prefabs = new();
        
        public TTypedPrefab GetPrefab<TTypedPrefab>() where TTypedPrefab : TPrefab
        {
            return prefabs.OfType<TTypedPrefab>().FirstOrDefault();
        }
    }
}