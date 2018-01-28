using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    Forward,
    Backward,
    Left,
    Right,
    ForwardLeft,
    ForwardRight,
    BackwardLeft,
    BackwardRight,
    Upwards,
    Downwards
}

[System.Serializable]
public class Room : MonoBehaviour {

    [System.Serializable]
    public class RoomDirection
    {
        public Direction _roomDirecion;
        [Range(0,10)]
        public int _xPadding;
        [Range(0, 10)]
        public int _yPadding;
        public Room _room;
        public bool _isLocked;
        
    }

    [System.Serializable]
    public struct SpawnRequest
    {
        public GameObject _spawnItem;
        public Transform _spawn;
    }

    public static Room[] _rootRooms = new Room[10];
    public static int _maxFloors = 1;

    public List<Transform> _hidingPoints;
    public List<Transform> _stopPoints;

    public List<SpawnRequest> _spawnRequests;

    public int _floor = 0;
    public List<CameraController> _cameraPoints;
    public RoomUIPoint _uiPoint;
    public List<RoomDirection> _roomNeighbours;
    public bool _isRoot;

    public ExitButton _exitButton;
    public List<Door> _doors;

    private List<Collider> _colliders;

    public string _tag;

    public void Awake()
    {
        if(_floor > _maxFloors-1)
        {
            _maxFloors = _floor + 1;
            if(_maxFloors > 10)
            {
                System.Array.Resize(ref _rootRooms, _maxFloors);
            }
        }
        if(_isRoot)
        {
            _rootRooms[_floor] = this;
        }
        InitaliseCameras();
    }

    public void Start()
    {
    }

    public void ProfessorEntered()
    {
        if (_uiPoint != null) {
            _uiPoint.ChangeColour(_uiPoint._professorColour);
        }
    }

    public void ProfessorExited()
    {
        if (_uiPoint != null) {
            _uiPoint.ChangeColour(_uiPoint._normalColour);
        }
    }

    private void InitaliseCameras()
    {
        foreach (CameraController camera in _cameraPoints)
        {
            if (camera != null) {
                camera.SetRoom(this);
            }
        }
    }

    public void ToggleLockNeighbour(Room room, bool locked)
    {
        for (int i = 0; i < _roomNeighbours.Count; i++)
        {
            RoomDirection r = _roomNeighbours[i];
            if (room == r._room && r._isLocked != locked)
            {
                r._isLocked = true;
            }
        }
    }

    public bool GetNeighbour(Direction direction, out Room resultRoom)
    {
        resultRoom = null;
        foreach (RoomDirection room in _roomNeighbours)
        {
            if(room._roomDirecion == direction)
            {
                resultRoom = room._room;
                return true;
            }
        }
        return false;
    }

    public Room GetRandomNeighbourRoom() {
        if (_roomNeighbours.Count > 0) {
            float random = Random.Range(0, _roomNeighbours.Count);
            int index = (int)random;
            return _roomNeighbours[index]._room;
        }
        else {
            Debug.LogError("No neighbours for room[" + name + "]");
            return null;
        }
    }

    public CameraController GetRandomCamera()
    {
        return _cameraPoints[(int)Random.Range(0, _cameraPoints.Count - 1)];
    }

    public Transform GetRandomPoint(List<Transform> xforms)
    {
        if (xforms.Count > 0) {
            int random = Random.Range(0, xforms.Count);
            return xforms[random];
        }
        else {
            return null;
        }
    }

    public Transform GetRandomStopPoint()
    {
        return GetRandomPoint(_stopPoints);
    }

    public Transform GetRandomHidingPoint()
    {
        return GetRandomPoint(_hidingPoints);
    }

    public Transform GetNearestHidingPoint(Transform target) {
        Transform nearestHidingPoint = null;
        float nearest = float.MaxValue;
        foreach (Transform hidingPoint in _hidingPoints) {
            float distance = Vector3.Distance(target.position, hidingPoint.position);
            if (distance < nearest) {
                nearest = distance;
                nearestHidingPoint = hidingPoint;
            }
        }

        return nearestHidingPoint;
    }

    public ExitButton GetExitButton() {
        return _exitButton;
    }

    public Door GetNearestDoor(Vector3 point) {
        Door nearest = null;
        float nearestDistance = float.MaxValue;
        foreach (Door door in _doors) {
            float distance = Vector3.Distance(point, door.transform.position);
            if (distance < nearestDistance) {
                nearestDistance = distance;
                nearest = door;
            }
        }

        return nearest;
    }

    private void OnTriggerEnter(Collider other)
    {
        RoomWalker walker = other.GetComponent<RoomWalker>();
        if (walker)
        {
            walker.OnEnterRoom(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        RoomWalker walker = other.GetComponent<RoomWalker>();
        if (walker)
        {
            walker.OnLeaveRoom(this);
        }
    }

}
