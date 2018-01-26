using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCameraManager : MonoBehaviour {
    
    public List<Camera> _cameras;
    
    public int _currentCamera;

    public CameraObject[] _cameras;

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
<<<<<<< refs/remotes/origin/master:Unity/Assets/Scripts/Player/RoomCameraManager.cs
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
=======
        Debug.Log("Camera Cycled Forwards");
    }

    public void CycleCameraBackward() {
        Debug.Log("Camera Cycled Backwards");
>>>>>>> NIGGAS DON'T KNOW SHIT ABOUT MY FLY ASS CAMERA UI DAWGGGGGGGGGGGGGGGGGGG:Unity/Assets/Scripts/Player/CameraManager.cs
    }
}
