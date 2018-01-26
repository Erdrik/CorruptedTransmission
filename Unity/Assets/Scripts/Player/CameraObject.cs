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
public class CameraObject : MonoBehaviour {

    [System.Serializable]
    public struct CameraDirection
    {
        public Direction _cameraDirecion;
        public CameraObject _camera;
    }

    public static CameraObject _rootCamera;

    public List<CameraDirection> _cameraNeighbours;
    public bool _isRoot;

    public void Awake()
    {
        if(_isRoot || _rootCamera == null)
        {
            _rootCamera = this;
        }
    }

    public bool GetNeighbour(Direction direction, out CameraObject resultCamera)
    {
        resultCamera = null;
        foreach (CameraDirection camera in _cameraNeighbours)
        {
            if(camera._cameraDirecion == direction)
            {
                resultCamera = camera._camera;
                return true;
            }
        }
        return false;
    }
	
}
