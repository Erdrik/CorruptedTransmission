using UnityEngine;
using System.Collections.Generic;

public class RoomCameraManager : MonoBehaviour {

    public static RoomCameraManager _instance;

    public Room _defaultRoom;

    public List<Room> _rooms;
    public Camera _mainCamera;
    public CameraUISelectionManager _cameraUIManager;

    private static Camera _mainStaticCamera;
    private static Room _activeRoom;

    private void Awake()

    {
        _activeRoom = null;
        _instance = this;

    }

    public void Start()
    {
        _mainStaticCamera = _mainCamera;

        GoToRoom(_defaultRoom);
    }

    public Room CurrentRoom {
        get {
            return _activeRoom;
        }
    }

    public void ChangeRoom(int index) {
        if (index < _rooms.Count) {
            GoToRoom(_rooms[index]);
        }
    }

    public static void GoToRoom(Room room)
    {
        if (room != null && 
            room._cameraPoints.Count > 0)
        {
            GoToCamera(room._cameraPoints[0]);
        }
    }

    public static void GoToCamera(CameraController camera) {
        if (_activeRoom != camera._owningRoom) {
            _instance._cameraUIManager.SetActiveRoom(camera._owningRoom);
            _activeRoom = camera._owningRoom;
        }
        _mainStaticCamera.transform.SetParent(camera.transform, false);
    }

}


        
