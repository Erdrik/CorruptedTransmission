using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Professor : MonoBehaviour {

    private Dictionary<string, Room> _knownRooms;

    public RoomWalker _roomWalker;

	// Use this for initialization
	void Start () {
        _knownRooms = new Dictionary<string, Room>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TeachRoom(string tag, Room room) {
        _knownRooms.Add(tag, room);
    }

    public void GoToRoom(Room room) {
        if (_knownRooms.ContainsKey(room._tag)) {
            _roomWalker.EnterRoom(_knownRooms[room._tag]);
        }
        else {
            Complain();
        }
    }

    public void GetOut() {
        _roomWalker.EnterRoom(_roomWalker._currentRoom.GetRandomNeighbourRoom());
    }

    public void GoBack() {
        _roomWalker.EnterPreviousRoom();
    }

    public void Stop() {
        _roomWalker.Stop();
    }

    public void Hide() {
        _roomWalker.MoveTowards(_roomWalker._currentRoom.GetRandomHidingPoint());
    }

    public void PushButton() {
        //_roomWalker.MoveTowards(_roomWalker._currentRoom.GetPushButtonPoint());
    }

    public void TouchWithDeath() {
        Debug.Log("The professor was touched by death!");
        _roomWalker.Stop();
    }

    private void Complain() {
        Debug.Log("The professor complains about something!");
    }
}
