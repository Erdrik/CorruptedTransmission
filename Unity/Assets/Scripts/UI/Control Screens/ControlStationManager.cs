using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlStationManager : MonoBehaviour {

    public List<ControlScreenManager> _controlScreens;
    public string _defaultMode;

    public void Awake()
    {
        //SetScreenMode(_defaultMode);
    }

    public void SetScreenMode(string mode)
    {
        foreach (ControlScreenManager screen in _controlScreens)
        {
            screen.SetScreenMode(mode);
        }
    }

    public void TurnOffAllScreens()
    {
        foreach (ControlScreenManager screen in _controlScreens)
        {
            screen.TurnOffAllScreens();
        }
    }
	
}
