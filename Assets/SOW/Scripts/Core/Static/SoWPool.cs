namespace SoW.Scripts.Core
{
    public static class SoWPool 
    {
        public static PoolSystem I;

        public static void RegisterPoolSystem(PoolSystem pool) => I = pool;
    }
}