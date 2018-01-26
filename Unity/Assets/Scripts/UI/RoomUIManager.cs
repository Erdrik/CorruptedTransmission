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

    public void Start()
    {
        BuildCameraUI();
    }

    public void BuildCameraUI()
    {
        Room room = Room._rootRoom;

        if (room)
        {
            Debug.Log(room);
            FindLastNode(room);
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
            doneRooms.Add(node);
            List<Room.RoomDirection> neighbours = node._roomNeighbours;
            RoomUIPoint point = BuildRoomUIPoint(position, node, 0);
            foreach (Room.RoomDirection n in neighbours)
            {
                switch (n._roomDirecion)
                {
                    case Direction.Forward:
                        point.Link(FindLastNode(n._room, position + new Vector2Int(0, 1*(n._padding+1)), doneRooms));
                        break;
                    case Direction.Backward:
                        point.Link(FindLastNode(n._room, position + new Vector2Int(0, -1 * (n._padding + 1)), doneRooms));
                        break;
                    case Direction.Left:
                        point.Link(FindLastNode(n._room, position + new Vector2Int(-1 * (n._padding + 1), 0), doneRooms));
                        break;
                    case Direction.Right:
                        point.Link(FindLastNode(n._room, position + new Vector2Int(1 * (n._padding + 1), 0), doneRooms));
                        break;
                    default:
                        break;
                }

            }
            point.SetRoom(node);
            point.BuildLinks();
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
