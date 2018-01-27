using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomUIPoint : MonoBehaviour {

    public LineRenderer _lineRender;
    private List<Vector3> _points = new List<Vector3>();
    private Room _room;

    public void Start()
    {
        GetComponent<Button>().onClick.AddListener(GoToRoom);
        transform.Translate(0, 0, -0.001f);
    }

    public void ClearLinks()
    {
        _points.Clear();
    }

    public void Link(RoomUIPoint c)
    {
        if (c != null)
        {
            _points.Add(c.GetComponent<RectTransform>().anchoredPosition3D - GetComponent<RectTransform>().anchoredPosition3D);
            
        }
    }

    public void BuildLinks()
    {
        List<Vector3> linePoints = new List<Vector3>
        {
            Vector3.zero
        };
        foreach (Vector3 point in _points)
        {
            linePoints.Add(point + new Vector3(0,0,3));
            linePoints.Add(Vector3.zero);
        }
        _lineRender.positionCount = linePoints.Count;
        _lineRender.SetPositions(linePoints.ToArray());
    }

    public void SetRoom(Room room)
    {
        _room = room;
    }

    public void GoToRoom()
    {
        Debug.Log("Click");
        if(_room != null)
        {
            RoomCameraManager.GoToRoom(_room);
        }
    }
	
}
