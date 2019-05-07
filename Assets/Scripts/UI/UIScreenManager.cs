using System;
using System.Collections.Generic;
using UnityEngine;

public class UIScreenManager : SingletonBehaviour<UIScreenManager>
{
    private UIScreen _CurrentScreen;
    private Dictionary<Type, UIScreen> _Screens;

    protected override void SingletonAwake()
    {
        _Screens = new Dictionary<Type, UIScreen>();

        foreach (var screen in GetComponentsInChildren<UIScreen>(true))
        {
            print("Added: "+ screen.name);
            screen.gameObject.SetActive(false);
            _Screens.Add(screen.GetType(), screen);
        }

        ShowScreen<MainMenuScreen>();
    }

    public void ShowScreen<T>() where T : UIScreen
    {
        if(_CurrentScreen != null)
        {
            _CurrentScreen.gameObject.SetActive(false);
        }

        var typeOf = typeof(T);
        if(_Screens.ContainsKey(typeOf) == false)
        {
            return;
        }
            
        _CurrentScreen = _Screens[typeOf];
        _CurrentScreen.gameObject.SetActive(true);
    }
}
