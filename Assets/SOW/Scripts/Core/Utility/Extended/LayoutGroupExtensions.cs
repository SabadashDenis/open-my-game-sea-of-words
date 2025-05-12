using UnityEngine;
using UnityEngine.UI;

namespace SoW.Scripts.Core.Utility.Extended
{
    public static class LayoutGroupExtensions
    {
        /// <summary>
        /// Returns grid fillable area size (without paddings & spacings)
        /// </summary>
        public static Vector2 GetUsefulSize(this VerticalLayoutGroup lg, RectTransform rt, int elementsCount)
        {
            var fullSize = rt.rect.size;
            var lgPaddings = new Vector2(lg.padding.horizontal,lg.padding.vertical); 
            var lgSpacings = new Vector2(0,  lg.spacing * elementsCount);

            return fullSize - lgSpacings - lgPaddings;
        }

        /// <summary>
        /// Returns grid fillable area size (without paddings & spacings)
        /// </summary>
        public static Vector2 GetUsefulSize(this HorizontalLayoutGroup lg, RectTransform rt, int elementsCount)
        {
            var fullSize = rt.rect.size;
            var lgPaddings = new Vector2(lg.padding.horizontal,lg.padding.vertical); 
            var lgSpacings = new Vector2(lg.spacing * elementsCount, 0);

            return fullSize - lgSpacings - lgPaddings;
        }
        
    }
}