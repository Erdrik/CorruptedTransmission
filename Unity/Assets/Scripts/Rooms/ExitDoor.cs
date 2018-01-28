using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : MonoBehaviour {

    public Door _door;

    public List<ExitLight> _lights;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Activate(int lightID) {
        if (lightID < _lights.Count) {
            _lights[lightID].Activate();
        }

        CheckActivated();
    }

    public bool IsActivated(int lightID) {
        if (lightID < _lights.Count) {
            return _lights[lightID].Activated;
        }
        else {
            return false;
        }
    }

    private void CheckActivated() {
        bool exitOpen = true;
        foreach (ExitLight light in _lights) {
            if (!light.Activated) {
                exitOpen = false;
                break;
            }
        }

        if (exitOpen) {
            _door.Unlock();
        }
    }
}
