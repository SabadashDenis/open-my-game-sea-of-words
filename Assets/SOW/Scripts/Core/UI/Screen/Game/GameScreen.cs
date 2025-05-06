using SoW.Scripts.Core.UI;
using SoW.Scripts.Core.UI.Screen.Game.Views;
using UnityEngine;

public class GameScreen : ScreenViewBase
{
    [SerializeField] private WordsGrid wordsGrid;
    [SerializeField] private LetterInputCircle inputCircle;
    [SerializeField] private LetterInputResult inputResult;
    
    public WordsGrid WordsGrid => wordsGrid;
    public LetterInputCircle InputCircle => inputCircle;
    public LetterInputResult InputResult => inputResult;
}
