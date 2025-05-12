using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SoW.Scripts.Core.UI.Screen.Game.Views
{
    public class WordsGrid : View
    {
        [SerializeField] private Transform linesRoot;
        [SerializeField] private VerticalLayoutGroup layoutGroup;
        
        private List<WordsGridLine> _wordLines = new ();
        public VerticalLayoutGroup LayoutGroup => layoutGroup;
        
        public void Clear()
        {
            foreach (var line in _wordLines)
            {
                line.Clear();
                SoWPool.I.WordGridLinesPool.Push(line);
            }
            
            _wordLines.Clear();
        }
        
        public WordsGridLine AddWord(string word)
        {
            var newWordLine = SoWPool.I.WordGridLinesPool.Pop<WordsGridLine>(linesRoot);
            newWordLine.SetupWord(word);
            _wordLines.Add(newWordLine);
            
            return newWordLine;
        }
    }
}