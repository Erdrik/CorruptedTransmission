﻿using System.Collections;
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
    BackwardRight
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

    public static Room _rootRoom;

    public List<Transform> _hidingPoints;
    public List<Transform> _stopPoints;

    public Transform _cameraPoints;
    public RoomUIPoint _uiPoint;
    public List<RoomDirection> _roomNeighbours;
    public bool _isRoot;

    public string _tag;

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

    public Room GetRandomNeighbourRoom() {
        return _roomNeighbours[Random.Range(0, _roomNeighbours.Count - 1)]._room;
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
