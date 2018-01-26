using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCameraManager : MonoBehaviour {
    
    public List<Camera> _cameras;
    
    public int _currentCamera;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public Camera CurrentCamera {
        get {
            return _cameras[_currentCamera];
        }
    }

    public void SetCamera(int cameraIndex) {
        if (cameraIndex >= 0 &&
            cameraIndex < _cameras.Count) {
            _currentCamera = cameraIndex;
        }
    }

    public void CycleCameraForward() {
        if (_currentCamera + 1 < _cameras.Count) {
            _currentCamera = _currentCamera + 1;
        }
        else {
            _currentCamera = 0;
        }
    }

    public void CycleCameraBackward() {
        if (_currentCamera - 1 >= 0) {
            _currentCamera = _currentCamera - 1;
        }
        else {
            _currentCamera = _cameras.Count - 1;
        }
    }
}
