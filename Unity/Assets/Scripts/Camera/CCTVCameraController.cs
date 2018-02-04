using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCTVCameraController : MonoBehaviour {

    private static CCTVCameraController _instance;

    private void Awake()
    {
        _instance = this;
    }

    public void GoToCamera(CameraController camera)
    {
        CameraUISelectionManager _cameraUI = GameManager.GetCameraUI();
        if (_cameraUI._activeRoom != camera._owningRoom)
        {
             _cameraUI.SetActiveRoom(camera._owningRoom);
        }
        _instance.transform.SetParent(camera.transform, false);
    }

}
