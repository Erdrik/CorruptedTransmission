using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCameraManager : MonoBehaviour {

    public List<Room> _rooms;
    public Camera _mainCamera;

    private static Camera _mainStaticCamera;

    public void Start()
    {
        _mainStaticCamera = _mainCamera;
    }

    public static void GoToRoom(Room room)
    {
        _mainStaticCamera.transform.SetParent(room._cameraPoints,false);
    }
    
}
