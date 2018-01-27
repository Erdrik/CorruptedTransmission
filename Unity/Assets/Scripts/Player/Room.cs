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
    public struct RoomDirection
    {
        public Direction _roomDirecion;
        [Range(0,10)]
        public int _xPadding;
        [Range(0, 10)]
        public int _yPadding;
        public Room _room;
        
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
            camera.SetRoom(this);
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
            return _roomNeighbours[Random.Range(0, _roomNeighbours.Count - 1)]._room;
        }
        else {
            Debug.LogError("No neighbours for room[" + name + "]");
            return null;
        }
    }

    public Vector3 GetRandomPoint(List<Transform> xforms)
    {
        return xforms[Random.Range(0, xforms.Count - 1)].position;
    }

    public Vector3 GetRandomStopPoint()
    {
        return GetRandomPoint(_stopPoints);
    }

    public Vector3 GetRandomHidingPoint()
    {
        return GetRandomPoint(_hidingPoints);
    }

}
