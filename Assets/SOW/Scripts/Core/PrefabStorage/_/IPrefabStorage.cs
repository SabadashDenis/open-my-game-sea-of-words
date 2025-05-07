using UnityEngine;

namespace SoW.Scripts.Core.PrefabStorage._
{
    public interface IPrefabStorage<TPrefab> where TPrefab : MonoBehaviour
    {
        TTypedPrefab GetPrefab<TTypedPrefab>() where TTypedPrefab : TPrefab;
    }
}