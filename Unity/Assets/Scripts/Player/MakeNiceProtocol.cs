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

    public void InstructMove() {
        _professor.InstructMove();
    }

    public void InstructRoom(int index) {
        _professor.InstructRoom(_roomCameraManager.GetRoom(index));
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

    public void InstructPushButton() {
        _professor.InstructActivate();
    }

}
