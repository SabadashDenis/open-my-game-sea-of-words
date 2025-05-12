using System;
using System.Collections.Generic;
using System.Linq;
using SoW.Scripts.Core.Factory._;
using SoW.Scripts.Core.Utility.Object.Initable;
using UnityEngine;


namespace SoW.Scripts.Core
{
    [Serializable]
    public class MonoPool<TObject> : InitableBehaviour, IPool<TObject>
        where TObject : MonoBehaviour
    {
        [SerializeField] private List<TObject> prefabs = new();

        private readonly Dictionary<Type, Stack<TObject>> _stack = new();
        
        protected override void InitInternal()
        {
            foreach (var prefab in prefabs)
            {
                _stack.Add(prefab.GetType(), new ());
            }
        }

        public TTypedObj Pop<TTypedObj>(Transform root = null) where TTypedObj : TObject
        {
            var objRoot = root ?? transform;
            
            if (TryGetStack<TTypedObj>(out var targetStack))
            {
                if (!targetStack.TryPop(out var resultObj))
                {
                    resultObj = Instantiate(GetPrefab<TTypedObj>(), objRoot);
                }

                resultObj.transform.SetParent(objRoot);
                resultObj.gameObject.SetActive(true);
                return (TTypedObj)resultObj;
            }

            return null;
        }

        public void Push(TObject obj)
        {
            obj.transform.SetParent(transform);
            obj.gameObject.SetActive(false);

            if (_stack.TryGetValue(obj.GetType(), out var targetStack))
            {
                targetStack.Push(obj);
            }
            else
            {
                Destroy(obj.gameObject);
            }
        }

        private bool TryGetStack<TTypedObj>(out Stack<TObject> resultStack) where TTypedObj : TObject
        {
            if (_stack.TryGetValue(typeof(TTypedObj), out var targetStack))
            {
                resultStack = targetStack;
                return true;
            }

            this.Log(LogType.Error, $"Pool does not contains prefab of type [{typeof(TTypedObj).Name}]");

            resultStack = null;
            return false;
        }

        private TTypedPrefab GetPrefab<TTypedPrefab>() where TTypedPrefab : TObject
        {
            return prefabs.OfType<TTypedPrefab>().FirstOrDefault();
        }
    }
}