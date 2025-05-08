namespace SoW.Scripts.Core.Factory._
{
    public static class SoWPool 
    {
        public static PoolSystem I;

        public static void RegisterPoolSystem(PoolSystem pool) => I = pool;
    }
}