using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitLight : MonoBehaviour {

    public Material _unactivatedMaterial;
    public Material _activatedMaterial;

    public MeshRenderer _renderer;

    public bool _activated;

	// Use this for initialization
	void Start () {
        ChangeMaterial();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Activate() {
        _activated = true;
        ChangeMaterial();
    }

    private void ChangeMaterial() {
        if (_renderer != null) {
            if (_activated) {
                _renderer.material = _activatedMaterial;
            }
            else {
                _renderer.material = _unactivatedMaterial;
            }
        }
    }

    public bool Activated {
        get {
            return _activated;
        }
    }
}
