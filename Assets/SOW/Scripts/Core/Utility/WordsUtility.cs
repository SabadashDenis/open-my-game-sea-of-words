using System.Collections.Generic;

namespace SoW.Scripts.Core.Utility
{
    public static class WordsUtility
    {
        public static string GetCommonLetters(string[] words)
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
                            result[wordLetter.Key] = wordLetter.Value;
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
            
            return lettersChain;
        }
    }
}