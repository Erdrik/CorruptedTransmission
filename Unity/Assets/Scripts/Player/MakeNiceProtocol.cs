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
	
	// Update is called once per frame
	void Update () {
        if (!_professor && RoomCameraManager._professor)
        {
            _professor = RoomCameraManager._professor;
        }
	}

    public void InstructMove() {
        _professor.InstructMove();
    }

    public void InstructRoom(int index) {
        _professor.InstructRoom(_roomCameraManager.GetRoom(index));
    }

    public void InstructRoom(Room room)
    {
        _professor.InstructRoom(room);
    }

    public void InstructGetOut() {
        _professor.InstructGetOut();
    }

    public void InstructGoBack() {
        _professor.InstructGoBack();
    }

    public void InstructStop() {
        _professor.InstructStop();
    }

    public void InstructHide() {
        _professor.InstructHide();
    }

    public void InstructActivate() {
        _professor.InstructActivate();
    }

    public void InstructDoor()
    {
        _professor.InstructDoor();
    }

}
