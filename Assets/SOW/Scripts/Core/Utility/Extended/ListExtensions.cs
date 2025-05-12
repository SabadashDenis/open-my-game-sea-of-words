using System.Collections.Generic;

namespace SoW.Scripts.Core.Utility.Extended
{
    public static class ListExtensions
    {
        /// <summary>
        /// Split list into equal parts: List[11] -> List[4] + List[4] + List[3] 
        /// </summary>
        public static List<List<T>> SplitList<T>(List<T> list, int numberOfParts)
        {
            List<List<T>> result = new List<List<T>>();
            int partSize = list.Count / numberOfParts;
            int remainder = list.Count % numberOfParts;

            int currentIndex = 0;
            for (int i = 0; i < numberOfParts; i++)
            {
                int size = partSize + (i < remainder ? 1 : 0);
                List<T> part = list.GetRange(currentIndex, size);
                result.Add(part);
                currentIndex += size;
            }

            return result;
        }
    }
}