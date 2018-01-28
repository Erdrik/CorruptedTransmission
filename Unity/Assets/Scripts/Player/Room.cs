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

    public static Room[] _rootRooms = new Room[10];
    public static int _maxFloors = 1;

    public List<Transform> _hidingPoints;
    public List<Transform> _stopPoints;

    public int _floor = 0;
    public List<CameraController> _cameraPoints;
    public RoomUIPoint _uiPoint;
    public List<RoomDirection> _roomNeighbours;
    public bool _isRoot;

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

}
