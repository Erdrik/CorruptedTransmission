using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoomWalker : MonoBehaviour {

    public NavMeshAgent _agent;

    public Room _currentRoom;
    public Room _previousRoom;
    public Room _nextRoom;

    public Vector3 _destinationPoint;

    public float _speed;
    public float _turnAngle;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        MoveTowards(_destinationPoint);
	}

    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward);

        Gizmos.color = Color.magenta;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Room")) {
            _currentRoom = other.GetComponentInParent<Room>();
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Room")) {
            _previousRoom = other.GetComponentInParent<Room>();
        }
    }

    public void Hide() {
        MoveTowards(_currentRoom.GetRandomHidingPoint());
    }

    public void EnterRoom(Room room) {
        if (room != null) {
            _nextRoom = room;
            Vector3 destination = room.GetRandomStopPoint();
            MoveTowards(destination);
        }
    }

    public void EnterPreviousRoom() {
        EnterRoom(_previousRoom);
    }

    public void MoveTowards(Vector3 target) {
        _destinationPoint = target;
        _agent.SetDestination(_destinationPoint);
    }

    public void Stop() {
        _destinationPoint = transform.position;
        _agent.SetDestination(_destinationPoint);
    }
}
