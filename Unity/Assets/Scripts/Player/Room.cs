using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    Forward,
    Backward,
    Left,
    Right
}

[System.Serializable]
public class Room : MonoBehaviour {

    [System.Serializable]
    public struct RoomDirection
    {
        public Direction _roomDirecion;
        [Range(0,10)]
        public int _padding;
        public Room _room;
    }

    public static Room _rootRoom;

    public List<Transform> _hidingPoints;
    public List<Transform> _stopPoints;

    public Transform _cameraPoints;
    public List<RoomDirection> _roomNeighbours;
    public bool _isRoot;

    public void Awake()
    {
        if(_isRoot || _rootRoom == null)
        {
            _rootRoom = this;
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
