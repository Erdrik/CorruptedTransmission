using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeNiceProtocol : MonoBehaviour {

    public Professor _professor;
    public RoomCameraManager _roomCameraManager;

	// Use this for initialization
	void Start () {
        foreach (Room room in _roomCameraManager._rooms) {
            _professor.TeachRoom(room._tag, room);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void InstructMoveToRoom(Room room) {
        _professor.GoToRoom(room);
    }

    public void InstructGetOut() {
        _professor.GetOut();
    }

    public void InstructGoBack() {
        _professor.GoBack();
    }

    public void InstructStop() {
        _professor.Stop();
    }

    public void InstructHide() {
        _professor.Hide();
    }

    public void InstructPushButton() {
        _professor.PushButton();
    }

}
