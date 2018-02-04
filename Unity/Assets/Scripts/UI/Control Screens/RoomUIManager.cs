using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomUIManager : MonoBehaviour {

    public RectTransform _rootPoint;
    public float _roomSpacing = 20;
    public float _roomSize = 60;
    public RoomUIPoint _RoomUIPointPrefab;
    public Text _floorText;

    private List<RoomUIPoint> _RoomUIPoints = new List<RoomUIPoint>();
    private Level _level;
    private int _currentFloor = 0;

    public void Awake()
    {
        GameManager._onLevelStart += OnLevelStart;
        
    }

    public void OnLevelStart(Level level)
    {
        Debug.Log("show level");
        _level = level;
        ShowFloor(0);
    }

    public void GoUpFloor()
    {
        if(_currentFloor < _level.GetNumberOfFloors() - 1)
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
        if (floor > -1 && floor < _level.GetNumberOfFloors())
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
        if (floor > -1 && floor < _level.GetNumberOfFloors())
        {
            Room room = _level._floors[floor]._rootRoom;

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
            _RoomUIPoints.Add(point);
        }
        if (!doneRooms.Contains(node))
        {
            List<Room.RoomConnection> neighbours = node._RoomConnections;
            
            doneRooms.Add(node);
            foreach (Room.RoomConnection n in neighbours)
            {
                
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
            point._text.text = position.x + "," + position.y;
        }
        return point;
    }

    

    private RoomUIPoint BuildRoomUIPoint(Vector2Int position, Room room, int i)
    {
        RoomUIPoint roomPoint = Instantiate<RoomUIPoint>(_RoomUIPointPrefab);
        RectTransform roomPointTransform = roomPoint.GetComponent<RectTransform>();
        roomPointTransform.SetParent(_rootPoint.GetComponent<RectTransform>(), false);
        roomPointTransform.anchoredPosition3D = new Vector2(position.x, position.y) * (_roomSpacing + _roomSize);
        //roomPointTransform.position = new Vector3(roomPointTransform.transform.position.x, roomPointTransform.transform.position.y, 0);
        return roomPoint;
    }

    

}
