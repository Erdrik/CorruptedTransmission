using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour {

    public List<Room> _rooms;
    public Room _endRoom;
    public Room _rootRoom;

    public Transform _professorSpawnPoint;
    public List<Transform> _enemySpawnPoints;

    public List<Nemesis> _enemies;

    public void TestFloorResidents()
    {
        List<RoomWalker> walkers = GetAllFloorWalkers();
        foreach (Room room in _rooms)
        {
            room.TestRoomForWalkers(walkers);
        }
    }

    public List<RoomWalker> GetAllFloorWalkers()
    {
        List<RoomWalker> walkers = new List<RoomWalker>();
        Professor p = GameManager._currentProfessor;
        if (p)
        {
            walkers.Add(p._roomWalker);
        }
        foreach (Nemesis n in _enemies)
        {
            walkers.Add(n._roomWalker);
        }
        return walkers;
    }

    public CameraController GetRandomCamera()
    {
        return _rooms[(int)Random.Range(0, _rooms.Count - 1)].GetRandomCamera();
    }

}
