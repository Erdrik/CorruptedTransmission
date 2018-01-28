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
        activate,
        lockDoor,
        actionLimit
    }

    public enum Emotion {
        distrust,
        anger,
        fear
    }

    private Dictionary<string, Room> _knownRooms = new Dictionary<string, Room>();

    public RoomWalker _roomWalker;
    public RoomWalker _nemesis;

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

    public float _waitingUntil;
    public float _minWait;
    public float _maxWait;
    public float _varianceWait;

    public Room _mentionedRoom;
    public Room _targetRoom;

    public Transform _reach;
    public ExitButton _button;

    // Use this for initialization
    void Start() {
        //_knownRooms = new Dictionary<string, Room>();
        RoomCameraManager.RegisterProfessor(this);
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
                case ProfessorAction.getOut:
                    GetOut();
                    break;
                case ProfessorAction.goBack:
                    GoBack();
                    break;
                case ProfessorAction.stop:
                    Stop();
                    break;
                case ProfessorAction.hide:
                    Hide();
                    break;
                case ProfessorAction.activate:
                    Activate();
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

    private float GetPatienceLength() {
        return Random.Range(_minWait, _maxWait) - (_distrust + (_anger * 4.0f) + (_fear * 2.0f));
    }

    private void ChangeAction(ProfessorAction action) {
        _actionState = ActionState.start;
        _currentActionType = action;
    }

    private void CompleteAction() {
        _actionState = ActionState.complete;
        _currentActionType = ProfessorAction.stop;
        Wait();

    }

    public void TeachRoom(string tag, Room room) {
        _knownRooms.Add(tag, room);
    }

    private bool NemesisIsInRoom() {
        return (_nemesis._currentRoom._tag == _roomWalker._currentRoom._tag);
    }

    private void Wait() {
        _waitingUntil = Time.time + GetPatienceLength();
    }

    private void Move() {
        switch (_actionState) {
            case ActionState.start:
                if (_mentionedRoom != null) {
                    if (_mentionedRoom._tag == _roomWalker._currentRoom._tag) {
                        AdjustEmotion(_minor, _minor, -_minor);
                        _mentionedRoom = null;
                        _roomWalker.Wander();
                        _actionState = ActionState.during;
                    }
                    else {
                        _targetRoom = _mentionedRoom;
                        _mentionedRoom = null;
                        _roomWalker.EnterRoom(_knownRooms[_targetRoom._tag]);
                        _actionState = ActionState.during;
                    }
                }
                else {
                    float currentTime = Time.time;
                    if (currentTime > _waitingUntil) {
                        switch (_greatestEmotion) {
                            case Emotion.distrust:
                                Complain("Where?");
                                Wait();
                                break;
                            case Emotion.anger:
                                Complain("Where?");
                                Wait();
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
                _targetRoom = _roomWalker._currentRoom.GetRandomNeighbourRoom();
                _roomWalker.EnterRoom(_targetRoom);
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
                    if (NemesisIsInRoom()) {
                        AdjustEmotion(-_minor, 0.0f, _moderate);
                    }
                    else {
                        AdjustEmotion(0.0f, 0.0f, _minor);
                    }
                }
                break;
        }
    }

    public void Activate() {
        switch (_actionState) {
            case ActionState.start:
                if (_button == null) {
                    _button = _roomWalker._currentRoom.GetExitButton();
                    if (_button == null) {
                        Complain("What button?");
                        AdjustEmotion(_minor, _minor, _minor);
                        CompleteAction();
                    }
                    else {
                        if (_button.Activated) {
                            Complain("I already pushed that one!");
                            AdjustEmotion(_moderate, _moderate, _minor);
                            CompleteAction();
                        }
                        else {
                            _roomWalker.MoveTowards(_button.transform.position);
                            _actionState = ActionState.during;
                        }
                    }
                }
                break;
            case ActionState.during:
                if (_button.Activated) {
                    AdjustEmotion(-_major, -_major, -_major);
                    _button = null;
                    CompleteAction();
                }
                else if (_roomWalker.AtDestination()) {
                    Debug.LogError("The professor did not push button when reached it!");
                    _button = null;
                    CompleteAction();
                }
                break;
        }

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
        else {
            ChangeAction(ProfessorAction.move);
            Wait();
        }
    }

    public void InstructRoom(Room room) {
        if (_currentActionType == ProfessorAction.move) {
            if (_targetRoom != null && 
                _targetRoom._tag == room._tag) {
                AdjustEmotion(0.0f, _minor, -_minor);
                Complain("Going there already!");
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
        ChangeAction(ProfessorAction.getOut);
    }

    public void InstructGoBack() {
        if (_roomWalker._previousRoom == null) {
            Complain("Where?");
        }
        ChangeAction(ProfessorAction.goBack);
    }

    public void InstructStop() {
        if (_currentActionType == ProfessorAction.stop) {
            Wait();
            Complain("I am staying still!");
        }
        else {
            ChangeAction(ProfessorAction.stop);
        }
    }

    public void InstructHide() {
        if (_currentActionType == ProfessorAction.hide) {
            Wait();
            AdjustEmotion(_minor, 0.0f, _minor);
        }
        else {
            ChangeAction(ProfessorAction.hide);
        }
    }

    public void InstructActivate() {
        ChangeAction(ProfessorAction.activate);
    }

    public void InstructLock() {
        ChangeAction(ProfessorAction.lockDoor);
    }

    public void TouchWithDeath() {
        Debug.Log("The professor was touched by death!");
        _roomWalker.Stop();
    }

    private void Complain(string complaint) {
        Debug.Log("The professor complains[" + complaint + "]");
    }
}
