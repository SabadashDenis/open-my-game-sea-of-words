using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SoW.Scripts.Core.UI.Screen.Game.Views
{
    public class WordsGrid : View
    {
        [SerializeField] private WordsGridLine linePrefab;
        [SerializeField] private Transform linesRoot;

        private Dictionary<string, WordsGridLine> _lines = new();

        [FoldoutGroup("API"), Button]
        public void SetupWords(string[] words)
        {
            Clear();
            
            foreach (var word in words)
            {
                AddWord(word);
            }
        }

        public void ShowWord(string word)
        {
            _lines[word].ShowWord().Forget();
        }
        
        private void AddWord(string word)
        {
            var newWordLine = Instantiate(linePrefab, linesRoot);
            newWordLine.SetupWord(word);
            
            _lines.Add(word, newWordLine);
        }

        private void Clear()
        {
            foreach (var line in _lines.Values)
            {
                line.Clear();
                Destroy(line.gameObject);
            }
            
            _lines.Clear();
        }
    }

    public struct LetterData
    {
        public char Letter;
        public int Count;
    }
}