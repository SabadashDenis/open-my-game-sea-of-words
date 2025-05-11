
namespace SoW.Scripts.Core.Save._
{
    public class SaveSystem : SystemBase
    {
        public static ISaver Saver { get; private set; }
        
        protected override void InitInternal(SystemData data)
        {
            Saver = new LocalSaver();
        }
    }
}