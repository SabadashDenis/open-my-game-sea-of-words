using UnityEngine;

namespace SoW.Scripts.Core.Factory._
{
    public interface IPool<TObject>
    {
        TTypedObj Pop<TTypedObj>(Transform root) where TTypedObj : TObject;
        void Push(TObject obj);
    }
}