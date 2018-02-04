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
    public class RoomConnection
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

    public Polygon2D _roomBounds;
    public static string PROPERTY_ROOMS_BOUNDS = "_roomBounds";

    public List<Transform> _hidingPoints;
    public List<Transform> _stopPoints;
    public List<CameraController> _cameraPoints;

    public RoomUIPoint _uiPoint;
    public List<RoomConnection> _RoomConnections;

    public bool _isRoot;
    public ExitButton _exitButton;
    public List<Door> _doors;

    private List<Collider> _colliders;

    public string _tag;

    public void Awake()
    {
        InitaliseCameras();
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
        for (int i = 0; i < _RoomConnections.Count; i++)
        {
            RoomConnection r = _RoomConnections[i];
            if (room == r._room && r._isLocked != locked)
            {
                r._isLocked = true;
            }
        }
    }

    public bool GetNeighbour(Direction direction, out Room resultRoom)
    {
        resultRoom = null;
        foreach (RoomConnection room in _RoomConnections)
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
        if (_RoomConnections.Count > 0) {
            float random = Random.Range(0, _RoomConnections.Count);
            int index = (int)random;
            return _RoomConnections[index]._room;
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

    /*
    public bool GoToCamera(CameraController camera, out CameraController result)
    {
        result = _cameraPoints.Find((CameraController c) => { return camera == c; });
        return (result != null);
    }*/

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
        foreach (Door door in _doors)
        {
            float distance = Vector3.Distance(point, door.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearest = door;
            }
        }
        return nearest;
    }

    public void TestRoomForWalkers(List<RoomWalker> walkers)
    {       
        foreach (RoomWalker walker in walkers)
        {
            Vector2 position2D = new Vector2(walker.transform.position.x, walker.transform.position.z) - new Vector2(transform.position.x, transform.position.z);
            if (_roomBounds.IsInPolygon(position2D))
            {
                walker.OnEnterRoom(this);
            }
        }
    }

}
