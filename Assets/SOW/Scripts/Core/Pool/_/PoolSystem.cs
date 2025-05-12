using SoW.Scripts.Core.Pool;
using UnityEngine;

namespace SoW.Scripts.Core
{
    public class PoolSystem : SystemBase
    {
        [SerializeField] private LettersPool lettersPool;
        [SerializeField] private WordGridsPool wordGridsPool;
        [SerializeField] private WordGridLinesPool wordGridLinesPool;
        
        public LettersPool LettersPool => lettersPool;
        public WordGridsPool WordGridsPool => wordGridsPool;
        public WordGridLinesPool WordGridLinesPool => wordGridLinesPool;

        protected override void InitInternal(SystemData data)
        {
            SoWPool.RegisterPoolSystem(this);
            
            lettersPool.Init();
            wordGridsPool.Init();
            wordGridLinesPool.Init();
        }
    }
}