using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomUIManager : MonoBehaviour {

    public RectTransform _rootPoint;
    public float _roomSpacing = 20;
    public float _roomSize = 60;

    public RoomCameraManager _roomManager;
    public RoomUIPoint _RoomUIPointPrefab;
    public Text _floorText;

    private List<RoomUIPoint> _RoomUIPoints = new List<RoomUIPoint>();
    private int _currentFloor = 0;

    public void Start()
    {
        ShowFloor(0);
    }

    public void GoUpFloor()
    {
        if(_currentFloor < Room._maxFloors - 1)
        {
            _currentFloor++;
            ShowFloor(_currentFloor);
        }
    }

    public void GoDownFloor()
    {
        if (_currentFloor > 0)
        {
            _currentFloor--;
            ShowFloor(_currentFloor);
        }
    }


    public void ShowFloor(int floor)
    {
        if (floor > -1 && floor < Room._maxFloors)
        {
            _floorText.text = "FLOOR " + floor;
            DeleteUI();
            BuildCameraUI(floor);
        }
    }

    void DeleteUI()
    {
        for (int i = 0; i < _RoomUIPoints.Count; i++)
        {
            Destroy(_RoomUIPoints[i].gameObject);
        }
        _RoomUIPoints.Clear();
    }

    public void BuildCameraUI(int floor)
    {
        if (floor > -1 && floor < Room._maxFloors)
        {
            Room room = Room._rootRooms[floor];

            if (room)
            {
                FindLastNode(room);
            }
        }
    }
    private void FindLastNode(Room node)
    {
        FindLastNode(node, Vector2Int.zero, new HashSet<Room>());
    }

    private RoomUIPoint FindLastNode(Room node, Vector2Int position, HashSet<Room> doneRooms)
    {
        if (!doneRooms.Contains(node))
        {
            List<Room.RoomDirection> neighbours = node._roomNeighbours;
            RoomUIPoint point;
            if (node._uiPoint)
            {
                point = node._uiPoint;
            }
            else
            {
                point = BuildRoomUIPoint(position, node, 0);
                point.SetRoom(node);
                node._uiPoint = point;
            }
            foreach (Room.RoomDirection n in neighbours)
            {
                doneRooms.Add(node);
                switch (n._roomDirecion)
                {
                    case Direction.Forward:
                        point.Link(FindLastNode(n._room, position + new Vector2Int(0, 1 * (n._yPadding + 1)), doneRooms));
                        break;
                    case Direction.Backward:
                        point.Link(FindLastNode(n._room, position + new Vector2Int(0, -1 * (n._yPadding + 1)), doneRooms));
                        break;
                    case Direction.Left:
                        point.Link(FindLastNode(n._room, position + new Vector2Int(-1 * (n._xPadding + 1), 0), doneRooms));
                        break;
                    case Direction.Right:
                        point.Link(FindLastNode(n._room, position + new Vector2Int(1 * (n._xPadding + 1), 0), doneRooms));
                        break;
                    case Direction.ForwardLeft:
                        point.Link(FindLastNode(n._room, position + new Vector2Int(-1 * (n._xPadding + 1), 1 * (n._yPadding + 1)), doneRooms));
                        break;
                    case Direction.ForwardRight:
                        point.Link(FindLastNode(n._room, position + new Vector2Int(1 * (n._xPadding + 1), 1 * (n._yPadding + 1)), doneRooms));
                        break;
                    case Direction.BackwardLeft:
                        point.Link(FindLastNode(n._room, position + new Vector2Int(-1 * (n._xPadding + 1), -1 * (n._yPadding + 1)), doneRooms));
                        break;
                    case Direction.BackwardRight:
                        point.Link(FindLastNode(n._room, position + new Vector2Int(1 * (n._xPadding + 1), -1 * (n._yPadding + 1)), doneRooms));
                        break;
                    default:
                        break;
                }

            }
            point.BuildLinks();
            _RoomUIPoints.Add(point);
            return point;
        }
        return null;
    }

    

    private RoomUIPoint BuildRoomUIPoint(Vector2Int position, Room room, int i)
    {
        RoomUIPoint roomPoint = Instantiate<RoomUIPoint>(_RoomUIPointPrefab);
        RectTransform roomPointTransform = roomPoint.GetComponent<RectTransform>();
        roomPointTransform.SetParent(GetComponent<RectTransform>(), false);
        roomPointTransform.anchoredPosition3D = new Vector2(position.x, position.y) * (_roomSpacing + _roomSize);
        //roomPointTransform.position = new Vector3(roomPointTransform.transform.position.x, roomPointTransform.transform.position.y, 0);
        return roomPoint;
    }

    

}
