using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeNiceProtocol : MonoBehaviour {

    [SerializeField]
    private Professor _professor;
    public const string PROPERTY_PROFESSOR = "_professor";

    [SerializeField]
    private RoomCameraManager _roomCameraManager;
    public const string PROPERTY_ROOM_CAMERA_MANAGER = "_roomCameraManager";

    [SerializeField]
    private int _selectedRoom;
    public const string PROPERTY_SELECTED_ROOM = "_selectedRoom";

	// Use this for initialization
	void Start () {
        foreach (Room room in _roomCameraManager._rooms) {
            _professor.TeachRoom(room._tag, room);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void InstructMoveToRoom() {
        _professor.GoToRoom(_roomCameraManager.CurrentRoom);
    }

    public void InstructMoveToRoom(int index) {
        _roomCameraManager.ChangeRoom(index);
        _professor.GoToRoom(_roomCameraManager.CurrentRoom);
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
