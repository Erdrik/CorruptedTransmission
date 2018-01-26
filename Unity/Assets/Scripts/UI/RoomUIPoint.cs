using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomUIPoint : MonoBehaviour {

    public LineRenderer _lineRender;
    private List<Vector3> _points = new List<Vector3>();
    private Room _room;

    public void ClearLinks()
    {
        _points.Clear();
    }

    public void Link(RoomUIPoint c)
    {
        if (c != null)
        {
            _points.Add(c.GetComponent<RectTransform>().position);
            
        }
    }

    public void BuildLinks()
    {
        List<Vector3> linePoints = new List<Vector3>
        {
            this.transform.position
        };
        foreach (Vector3 point in _points)
        {
            linePoints.Add(point);
            linePoints.Add(this.transform.position);
        }
        Debug.Log(linePoints.ToArray());
        _lineRender.positionCount = linePoints.Count;
        _lineRender.SetPositions(linePoints.ToArray());
    }

    public void SetRoom(Room room)
    {
        _room = room;
    }

    public void GoToRoom()
    {
        if(_room != null)
        {
            RoomCameraManager.GoToRoom(_room);
        }
    }
	
}
