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

    public List<Room> _rooms;
    public Camera _mainCamera;
    public CameraUISelectionManager _cameraUIManager;

    public List<GameObject> _menuScreens;
    public List<GameObject> _gameScreens;

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
        StopGameScreens();
        RunMenuScreens();
        GoToRoom(_defaultRoom);
    }

    public void Update()
    {
        
    }

    public void RunGameScreens()
    {
        foreach (GameObject screen in _gameScreens)
        {
            screen.SetActive(true);
        }
    }

    public void StopGameScreens()
    {
        foreach (GameObject screen in _gameScreens)
        {
            screen.SetActive(false);
        }
    }

    public void RunMenuScreens()
    {
        foreach (GameObject screen in _menuScreens)
        {
            screen.SetActive(true);
        }
    }

    public void StopMenuScreens()
    {
        foreach (GameObject screen in _menuScreens)
        {
            screen.SetActive(false);
        }
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

    public void SpawnRoomSpawnables()
    {
        foreach (Room r in _instance._rooms)
        {
            foreach (Room.SpawnRequest requst in r._spawnRequests)
            {
                GameObject obj = Instantiate(requst._spawnItem);
                obj.transform.position = requst._spawn.position;
            }
        }
    }

    public static void RegisterProfessor(Professor p)
    {
        _professor = p;
        _professor._roomWalker._currentRoom = _instance._defaultRoom;
        CheckIfSpawned();
    }

    public static void RegisterNemisis(Nemesis n)
    {
        _nemisis = n;
        CheckIfSpawned();
    }

    private static void CheckIfSpawned()
    {
        if(_professor && _nemisis && _gameState == GameState.Menu)
        {
            _gameState = GameState.Playing;
            _instance.RunGameScreens();
            _instance.StopMenuScreens();
            foreach (Room room in _instance._rooms)
            {
                
                _professor.TeachRoom(room._tag, room);
            }
        }
    }

    public static void EndGame()
    {
        Destroy(_professor.gameObject);
        Destroy(_nemisis.gameObject);
        _gameState = GameState.Menu;
        _instance.StopGameScreens();
        _instance.RunMenuScreens();
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


        
