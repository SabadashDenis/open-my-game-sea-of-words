using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace SoW.Scripts.Core.UI.Screen.Game.Views
{
    public class WordsGrid : View
    {
        [SerializeField] private WordsGridLine linePrefab;
        [SerializeField] private Transform linesRoot;
        [SerializeField] private VerticalLayoutGroup layoutGroup;
        public VerticalLayoutGroup LayoutGroup => layoutGroup;
        
        private Dictionary<string, WordsGridLine> _words = new ();
        
        public IReadOnlyDictionary<string, WordsGridLine> Words => _words;
        
        [FoldoutGroup("API"), Button]
        public void SetupWords(string[] words)
        {
            foreach (var word in words)
            {
                AddWord(word);
            }
        }

        public void Clear()
        {
            foreach (var line in _words.Values)
            {
                line.Clear();
                Destroy(line.gameObject);
            }
            
            _words.Clear();
        }
        
        private void AddWord(string word)
        {
            var newWordLine = Instantiate(linePrefab, linesRoot);
            newWordLine.SetupWord(word);
            _words.Add(word, newWordLine);
        }
    }
}