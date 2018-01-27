using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum WalkerState {
    atDestination,
    moving
}

public class RoomWalker : MonoBehaviour {

    public NavMeshAgent _agent;

    public Room _currentRoom;
    public Room _previousRoom;
    public Room _nextRoom;

    public Vector3 _destinationPoint;
    public WalkerState _state;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        if (_state == WalkerState.moving) {
            if (Arrived()) {
                _state = WalkerState.atDestination;
            }
        }
	}

    void OnDrawGizmos() {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward);
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

    public bool AtDestination() {
        return _state == WalkerState.atDestination;
    }

    private bool Arrived() {
        bool notMoving = !Moving();
        bool noPath = !_agent.hasPath;
        bool noVelocity = _agent.velocity.sqrMagnitude == 0.0f;
        return notMoving && noPath && noVelocity;
    }

    public bool Moving() {
        return !_agent.pathPending &&
            _agent.path.status == NavMeshPathStatus.PathComplete &&
            _agent.remainingDistance > _agent.stoppingDistance;
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
        _state = WalkerState.moving;
        _destinationPoint = target;
        _agent.SetDestination(_destinationPoint);
    }

    public void Stop() {
        _state = WalkerState.atDestination;
        _destinationPoint = transform.position;
        _agent.SetDestination(_destinationPoint);
    }
}
