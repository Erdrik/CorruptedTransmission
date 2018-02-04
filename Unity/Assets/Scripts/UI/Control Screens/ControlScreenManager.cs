using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ControlScreenManager : MonoBehaviour {

    [System.Serializable]
    public class ControlScreenMode
    {
        public string _name;
        public List<GameObject> _screenObjects;
    }

    public List<ControlScreenMode> _screenModes;
    [SerializeField]
    private ControlScreenMode _currentMode;

    public void Awake()
    {
        TurnOffAllScreens();
    }

    public void SetScreenMode(string modeName)
    {
        ControlScreenMode selectedMode;
        if(modeName == "")
        {
            TurnOffAllScreens();
            _currentMode = new ControlScreenMode();
            return;
        }
        if (FindScreenMode(modeName, out selectedMode)) {
            if (_currentMode != null && _currentMode._name != null)
            {
                SetScreenObjects(_currentMode, false);
            }
            SetScreenObjects(selectedMode, true);
            _currentMode = selectedMode;
        }
    }
    
    public void TurnOffAllScreens()
    {
        foreach (ControlScreenMode mode in _screenModes)
        {
            SetScreenObjects(mode, false);
        }
    }

    private bool FindScreenMode(string mode, out ControlScreenMode result)
    {
        result = _screenModes.Find((ControlScreenMode c) => { return c._name == mode; });
        return (result != null);
    }

    private void SetScreenObjects(ControlScreenMode mode, bool active)
    {
        foreach (GameObject go in mode._screenObjects)
        {
            go.SetActive(active);
        }
    }
	
}
