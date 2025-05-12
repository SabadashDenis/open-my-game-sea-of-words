using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using SoW.Scripts.Core;
using SoW.Scripts.Core.UI;
using UnityEngine;
using LogType = SoW.Scripts.Core.LogType;

public class UISystem : SystemBase
{
    [SerializeField] private List<ScreenViewBase> screens = new();

    protected override void InitInternal(SystemData data)
    {
        foreach (var screen in screens)
        {
            screen.HideImmediately();
        }
    }
    
    public TScreen GetScreen<TScreen>() where TScreen : ScreenViewBase
    {
        if (screens.Count > 0)
        {
            var resultScreen = screens.FirstOrDefault(screen => screen is TScreen);
            
            if(resultScreen != null)
                return resultScreen as TScreen;
        }
        
        this.Log(LogType.Error, $"Can't get screen of type {typeof(TScreen).Name}");
        return null;
    }
    
    [Button]
    private void Collect() => screens = GetComponentsInChildren<ScreenViewBase>().ToList();
}
