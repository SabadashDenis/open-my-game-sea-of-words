using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SoW.Scripts.Core.UI.Screen.Game.Views
{
    public class WordsGrid : View
    {
        [SerializeField] private WordsGridLine linePrefab;
        [SerializeField] private Transform linesRoot;

        private List<WordsGridLine> _lines = new();

        public string LettersChain { get; private set; }

        [FoldoutGroup("API"), Button]
        public void SetupWords(string[] words)
        {
            Clear();
            
            foreach (var word in words)
            {
                AddWord(word);
            }
            
            SetupLettersChain(words);
        }

        private void SetupLettersChain(string[] words)
        {
            Dictionary<char, int> result = new();
            
            foreach (string word in words)
            {
                Dictionary<char, int> wordLetters = new();
                
                foreach (char letter in word)
                {
                    if (!wordLetters.TryAdd(letter, 1))
                    {
                        wordLetters[letter]++;
                    }
                }

                foreach (var wordLetter in wordLetters)
                {
                    if (result.TryGetValue(wordLetter.Key, out var resultLettersCount))
                    {
                        if(resultLettersCount < wordLetter.Value)
                            result[wordLetter.Key] = resultLettersCount;
                    }
                    else
                    {
                        result.Add(wordLetter.Key, wordLetter.Value);
                    }
                }
            }

            string lettersChain = string.Empty;

            foreach (var letterData in result)
            {
                for (int i = 0; i < letterData.Value; i++)
                {
                    lettersChain += letterData.Key;
                }
            }
            
            LettersChain = lettersChain;
        }

        private void AddWord(string word)
        {
            var newWordLine = Instantiate(linePrefab, linesRoot);
            newWordLine.SetupWord(word);
            
            _lines.Add(newWordLine);
        }

        public void Clear()
        {
            foreach (var line in _lines)
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