using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCameraManager : MonoBehaviour {

    public List<Room> _rooms;
    public Camera _mainCamera;
    public CameraUISelectionManager _cameraUIManager;

    private static Camera _mainStaticCamera;
    private static Room _activeRoom;

    public void Start()
    {
        _mainStaticCamera = _mainCamera;
    }

    public static void GoToRoom(Room room)
    {
        _mainStaticCamera.transform.SetParent(room._cameraPoints,false);
    }
    
    public static void GoToCamera(CameraController camera)
    {
        if(_activeRoom != camera._owningRoom)
        {
            _instance._cameraUIManager.SetActiveRoom(camera._owningRoom);
            _activeRoom = camera._owningRoom;
        }
        _mainStaticCamera.transform.SetParent(camera.transform, false);

    }

}

public class RoomCameraManager : MonoBehaviour {

    public static RoomCameraManager _instance;

    public Room _defaultRoom;
    private void Awake()
    {
        _instance = this;
    }

    public void Start()
    {
        _mainStaticCamera = _mainCamera;
        GoToRoom(_defaultRoom);
    public static void GoToRoom(Room room)
    {
        if(room._cameraPoints.Count > 0)
        {
            GoToCamera(room._cameraPoints[0]);
        }