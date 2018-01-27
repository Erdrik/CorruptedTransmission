using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomUIPoint : MonoBehaviour {

    public LineRenderer _lineRender;
    private List<Vector3> _points = new List<Vector3>();
    private Room _room;

    public float bufferA = -0.01f;
    public float bufferB = 3;

    public void Start()
    {
        GetComponent<Button>().onClick.AddListener(GoToRoom);
        transform.Translate(0, 0, bufferA);
    }

    public void ClearLinks()
    {
        _points.Clear();
    }

    public void Link(RoomUIPoint c)
    {
        if (c != null)
        {
            _points.Add(c.GetComponent<RectTransform>().anchoredPosition3D - GetComponent<RectTransform>().anchoredPosition3D+ new Vector3(0,0, bufferB));
            
        }
    }

    public void BuildLinks()
    {
        List<Vector3> linePoints = new List<Vector3>
        {
            Vector3.zero+ new Vector3(0,0,bufferB)
        };
        foreach (Vector3 point in _points)
        {
            linePoints.Add(point + new Vector3(0,0, bufferB));
            linePoints.Add(Vector3.zero + new Vector3(0, 0, bufferB));
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
        GetComponent<Button>().OnDeselect(new UnityEngine.EventSystems.BaseEventData(UnityEngine.EventSystems.EventSystem.current));
        if (_room != null)
        {
            RoomCameraManager.GoToRoom(_room);
        }
    }
	
}
