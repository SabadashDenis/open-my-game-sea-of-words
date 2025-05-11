using UnityEngine;
using UnityEngine.UI;

namespace SoW.Scripts.Core.Utility.Extended
{
    public static class LayoutGroupExtensions
    {
        public static Vector2 GetUsefulSize(this VerticalLayoutGroup lg, int elementsCount)
        {
            var rectTr = lg.GetComponent<RectTransform>();
            
            var fullSize = rectTr.rect.size;
            var lgPaddings = new Vector2(lg.padding.horizontal,lg.padding.vertical); 
            var lgSpacings = new Vector2(0,  lg.spacing * elementsCount);

            return fullSize - lgSpacings - lgPaddings;
        }

        public static Vector2 GetUsefulSize(this HorizontalLayoutGroup lg, int elementsCount)
        {
            var rectTr = lg.GetComponent<RectTransform>();
            
            var fullSize = rectTr.rect.size;
            var lgPaddings = new Vector2(lg.padding.horizontal,lg.padding.vertical); 
            var lgSpacings = new Vector2(lg.spacing * elementsCount, 0);

            return fullSize - lgSpacings - lgPaddings;
        }
        
    }
}