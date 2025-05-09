
using SoW.Scripts.Core.Pool;
using UnityEngine;

namespace SoW.Scripts.Core
{
    public class PoolSystem : SystemBase
    {
        [SerializeField] private LettersPool lettersPool;
        
        public LettersPool LettersPool => lettersPool;

        protected override void InitInternal(SystemData data)
        {
            SoWPool.RegisterPoolSystem(this);
        }
    }
}