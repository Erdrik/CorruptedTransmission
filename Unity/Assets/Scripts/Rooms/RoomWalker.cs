using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoomWalker : MonoBehaviour {

    public NavMeshAgent _agent;

    public Room _currentRoom;
    public Vector3 _nextDestination;

    public float _speed;
    public float _turnAngle;

	// Use this for initialization
	void Start () {
        EnterRoom(_currentRoom);
	}
	
	// Update is called once per frame
	void Update () {
        MoveTowards(_nextDestination);
	}

    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward);

        Gizmos.color = Color.magenta;
    }

    public void Hide() {
        MoveTowards(_currentRoom.GetRandomHidingPoint());
    }

    public void EnterRoom(Room room) {
        Vector3 destination = room.GetRandomStopPoint();
        MoveTowards(destination);
    }

    public void MoveTowards(Vector3 target) {
        _nextDestination = target;
        _agent.SetDestination(_nextDestination);
    }
}
