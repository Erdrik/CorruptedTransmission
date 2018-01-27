using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Professor : MonoBehaviour {

    public enum ProfessorAction {
        move,
        getOut,
        goBack,
        hide,
        stop,
        actionLimit
    }

    public enum Emotion {
        distrust,
        anger,
        fear
    }

    private Dictionary<string, Room> _knownRooms;

    public RoomWalker _roomWalker;

    public ActionState _actionState;
    public ProfessorAction _currentActionType;

    public ProfessorAction _previousActionType;
    public float _lastInstruction;

    [Range(0.0f, 1.0f)]
    public float _distrust;
    [Range(0.0f, 1.0f)]
    public float _anger;
    [Range(0.0f, 1.0f)]
    public float _fear;

    public Emotion _greatestEmotion;

    [Range(0.0f, 1.0f)]
    public float _minor;
    [Range(0.0f, 1.0f)]
    public float _moderate;
    [Range(0.0f, 1.0f)]
    public float _major;

    float _patience;
    public Room _mentionedRoom;
    public Room _targetRoom;

    // Use this for initialization
    void Start() {
        _knownRooms = new Dictionary<string, Room>();
        Stop();
    }

    // Update is called once per frame
    void Update() {
        Think();
        if (_actionState == ActionState.complete) {
            Impatient();
        }
        else {
            switch (_currentActionType) {
                case ProfessorAction.move:
                    Move();
                    break;
                case ProfessorAction.stop:
                    Stop();
                    break;
                case ProfessorAction.actionLimit:
                default:
                    Debug.LogError("Invalid ProfessorAction![" + _currentActionType + "]");
                    break;
            }
        }
    }

    private void Think() {
        if (_distrust > _anger && _distrust > _fear) {
            _greatestEmotion = Emotion.distrust;
        }
        else if (_anger > _distrust && _anger > _fear) {
            _greatestEmotion = Emotion.anger;
        }
        else if (_fear > _distrust && _fear > _distrust) {
            _greatestEmotion = Emotion.fear;
        }
    }

    private void Impatient() {

    }

    private Emotion GreatestEmotion {
        get {
            return _greatestEmotion;
        }
    }

    private void AdjustEmotion(float distrust, float anger, float fear) {
        _distrust = Mathf.Clamp01(_distrust + distrust);
        _anger = Mathf.Clamp01(_anger + anger);
        _fear = Mathf.Clamp01(_fear + fear);
    }

    private void CompleteAction() {
        _actionState = ActionState.complete;
    }

    public void TeachRoom(string tag, Room room) {
        _knownRooms.Add(tag, room);
    }

    private void Move() {
        switch (_actionState) {
            case ActionState.start:
                if (_mentionedRoom != null) {
                    _mentionedRoom = null;
                    _targetRoom = _mentionedRoom;
                    _roomWalker.EnterRoom(_knownRooms[_mentionedRoom._tag]);
                }
                else {
                    float currentTime = Time.time;
                    if (currentTime > _patience) {
                        switch (_greatestEmotion) {
                            case Emotion.distrust:
                                Complain("Where?");
                                break;
                            case Emotion.anger:
                                Complain("Where?");
                                break;
                            case Emotion.fear:
                                GetOut();
                                break;
                            default:
                                Debug.LogError("Invalid emotion![" + GreatestEmotion + "]");
                                break;

                        }
                    }
                }
                break;
            case ActionState.during:
                if (_roomWalker.AtDestination()) {
                    CompleteAction();
                }
                break;
        }
    }

    public void GetOut() {
        switch (_actionState) {
            case ActionState.start:
                _roomWalker.EnterRoom(_roomWalker._currentRoom.GetRandomNeighbourRoom());
                _actionState = ActionState.during;
                break;
            case ActionState.during:
                if (_roomWalker.AtDestination()) {
                    CompleteAction();
                }
                break;
        }
    }

    public void GoBack() {
        switch (_actionState) {
            case ActionState.start:
                _roomWalker.EnterPreviousRoom();
                _actionState = ActionState.during;
                break;
            case ActionState.during:
                if (_roomWalker.AtDestination()) {
                    CompleteAction();
                }
                break;
        }
    }

    public void Stop() {
        switch (_actionState) {
            case ActionState.start:
                _roomWalker.Stop();
                _actionState = ActionState.complete;
                break;
            case ActionState.during:
                break;
        }
    }

    public void Hide() {
        switch (_actionState) {
            case ActionState.start:
                AdjustEmotion(_minor, _minor, _moderate);
                _roomWalker.Hide();
                _actionState = ActionState.during;
                break;
            case ActionState.during:
                if (_roomWalker.AtDestination()) {
                    AdjustEmotion(-_minor, 0.0f, -_minor);
                    CompleteAction();
                }
                break;
        }
    }

    public void PushButton() {
        //_pushButton = _roomWalker._currentRoom.GetPushButton();
        //if (_pushButton == null) {
        //    Complain();
        //}
        //else if (Vector3.Distance(transform.position, _pushButton.transform.position) > _touchDistance) {

        //}
        //else {
        //    _roomWalker.MoveTowards(_pushButton.ActivatePoint);
        //}
    }

    public void RepeatedInstruction() {
        switch (_greatestEmotion) {
            case Emotion.distrust:
                AdjustEmotion(_minor, _moderate, _minor);
                break;
            case Emotion.anger:
                AdjustEmotion(0.0f, _minor, 0.0f);
                break;
            case Emotion.fear:
                AdjustEmotion(_minor, _minor, 0.0f);
                break;
        }
        Complain("You said that already!");
    }

    public void InstructMove() {
        if (_currentActionType == ProfessorAction.move &&
            _actionState == ActionState.start) {
            int random = Random.Range(0, 2);
            switch (random) {
                case 0:
                    Complain("Where?");
                    Stop();
                    break;
                case 1:
                    Complain("You said that already!");
                    RepeatedInstruction();
                    break;
            }
        }
    }

    public void InstructRoom(Room room) {
        if (_currentActionType == ProfessorAction.move) {
            if (_targetRoom._tag == room._tag) {
                RepeatedInstruction();
            }
            else if (_knownRooms.ContainsKey(room._tag)) {
                _mentionedRoom = room;
            }
            else {
                Complain("What room is that?");
            }
        }
        else {
            Complain("What about it?");
        }
    }

    public void InstructGetOut() {
        _currentActionType = ProfessorAction.move;
    }

    public void InstructGoBack() {

    }

    public void InstructStop() {

    }

    public void InstructHide() {

    }

    public void InstructActivate() {

    }

    public void TouchWithDeath() {
        Debug.Log("The professor was touched by death!");
        _roomWalker.Stop();
    }

    private void Complain(string complaint) {
        Debug.Log("The professor complains[" + complaint + "]");
    }
}
