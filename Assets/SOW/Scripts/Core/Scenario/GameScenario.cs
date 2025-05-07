using System.Collections.Generic;
using Sirenix.OdinInspector;
using SoW.Scripts.Core.Pool;
using SoW.Scripts.Core.Scenario._;
using SoW.Scripts.Core.UI.Screen.Game.Views.LetterView;
using UnityEngine;

namespace SoW.Scripts.Core.Scenario
{
    public class GameScenario : ScenarioBase
    {
        [SerializeField] private LettersPool lettersPool;
        
        private GameScreen _gameScreen;
        private List<char> _currentInput = new();

        [FoldoutGroup("API"), Button]
        private void ClearLetters()
        {
            var letters = FindObjectsByType<LetterView>(FindObjectsInactive.Include, FindObjectsSortMode.None);

            foreach (var letterView in letters)
            {
                Destroy(letterView.gameObject);
            }
        }
        
        [FoldoutGroup("API"), Button]
        private void TestPopLetter(Transform root)
        {
            lettersPool.Pop<LetterView>(root);
        }
        
        [FoldoutGroup("API"), Button]
        private void TestPopSelectable(Transform root)
        {
            lettersPool.Pop<SelectableLetterView>(root);
        }

        [FoldoutGroup("API"), Button]
        private void TestPushLetter(LetterView obj)
        {
            lettersPool.Push(obj);
        }
        
        protected override void InitInternal(ScenarioData data)
        {
            _gameScreen = data.UI.GetScreen<GameScreen>();
        }

        protected override void PlayInternal()
        {
            _gameScreen.Show();

            _gameScreen.InputCircle.OnInputChanged += ProcessLetterInput;
            _gameScreen.WordsGrid.SetupWords(new[]{"канон","икона","цинк","кино","ион","инок"});
            _gameScreen.InputCircle.SetupLetters(_gameScreen.WordsGrid.LettersChain);
        }

        private void ProcessLetterInput(char letter, bool needToAdd)
        {
            if (needToAdd)
            {
                _currentInput.Add(letter);
                _gameScreen.InputResult.Append(letter);
            }
            else
            {
                _currentInput.Remove(letter);
                _gameScreen.InputResult.RemoveLast();
            }
        }
        
        protected override void OnTick()
        {
        }

        protected override void StopInternal()
        {
            _gameScreen.InputCircle.OnInputChanged -= ProcessLetterInput;
        }
    }
}