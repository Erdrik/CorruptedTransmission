using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {

    public List<Floor> _floors;
    public int _activeFloor = 0;

    public int _nextLevelScene;

    private void Start()
    {
        GameManager.RegisterLevel(this);
    }

    private void Update()
    {
        TestActiveFloorResidents(0);
    }

    public void SpawnProfessor(int i)
    {
        if (i > -1 && i < GetNumberOfFloors() && _floors[i] != null)
        {
            Floor floor = _floors[_activeFloor];
            Professor professor = Instantiate(GameManager.GetProfessorPrefab(), floor._professorSpawnPoint.transform.position, Quaternion.identity);
            GameManager._currentProfessor = professor;
        }
    }

    public int GetNumberOfFloors()
    {
        return _floors.Count;
    }
	
    public void TestActiveFloorResidents(int i)
    {
        _floors[i].TestFloorResidents();
    }

    public void DeregisterLevel()
    {

    }

}
