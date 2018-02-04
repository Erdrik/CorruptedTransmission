using UnityEngine;
using System.Collections.Generic;

public class RoomCameraManager : MonoBehaviour {

    public enum GameState
    {
        Menu,
        Spawned,
        Playing,
        Paused,
        End
    }

    public static RoomCameraManager _instance;

    public Room _defaultRoom;
    public Room _defaultNemisisRoom;

    public List<Room> _rooms;
    public Camera _mainCamera;
    public CameraUISelectionManager _cameraUIManager;

    public Room _profSpawnRoom;

    public static Professor _professor;
    public static Nemesis _nemisis;

    public static GameState _gameState;

    private static Camera _mainStaticCamera;
    private static Room _activeRoom;

    private void Awake()

    {
        _activeRoom = null;
        _instance = this;
        _gameState = GameState.Menu;

    }

    public void Start()
    {
        _mainStaticCamera = _mainCamera;
        GoToRoom(_defaultRoom);
    }

    public void Update()
    {
        
    }

    public Room CurrentRoom {
        get {
            return _activeRoom;
        }
    }

    public Room GetRoom(int index) {
        if (index < _rooms.Count) {
            return _rooms[index];
        }
        else {
            return null;
        }
    }

    public static void EndGame()
    {
        Destroy(_professor.gameObject);
        Destroy(_nemisis.gameObject);
        _gameState = GameState.Menu;
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


        
