using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nemesis : MonoBehaviour {

    public enum NemesisSpooky {
        wait,
        jitter,
        stare,
        spookyLimit
    }

    public enum NemesisAction {
        explore,
        search,
        hunt,
        spooky,
        actionLimit
    }

    public enum ActionState {
        start,
        during,
        complete
    }

    public RoomWalker _roomWalker;
    public RoomWalker _victim;

    public ActionState _actionState;
    public NemesisAction _currentActionType;
    public NemesisSpooky _lastSpooky;
    public float _nextActionTime;

    public float _minWait;
    public float _maxWait;

	// Use this for initialization
	void Start () {
        _actionState = ActionState.complete;
	}
	
	// Update is called once per frame
	void Update () {
        if (_actionState == ActionState.complete) {
            float currentTime = Time.time;
            if (currentTime >= _nextActionTime) {
                Wait();
                _actionState = ActionState.start;
            }
        }
        else {
                switch (_currentActionType) {
                case NemesisAction.explore:
                    Explore();
                    break;
                case NemesisAction.search:
                    Search();
                    break;
                case NemesisAction.hunt:
                    Hunt();
                    break;
                case NemesisAction.spooky:
                    NemesisSpooky spooky = (NemesisSpooky)Random.Range(0, (int)NemesisSpooky.spookyLimit);
                    while (spooky != _lastSpooky) {
                        spooky = (NemesisSpooky)Random.Range(0, (int)NemesisSpooky.spookyLimit);
                    }
                    DoSpooky(spooky);
                    break;
                case NemesisAction.actionLimit:
                default:
                    Debug.LogError("Invalid NemesisAction![" + _currentActionType + "]");
                    break;
            }
        }
	}

    private void CompleteAction() {
        _actionState = ActionState.complete;
        _currentActionType = NemesisAction.spooky;
    }

    private void CompleteSpooky() {
        _actionState = ActionState.complete;
        _currentActionType = (NemesisAction)Random.Range(0, (int)NemesisAction.actionLimit);
    }

    private void Explore() {
        if (_actionState == ActionState.start) {
            _roomWalker.EnterRoom(_roomWalker._currentRoom.GetRandomNeighbourRoom());
            _actionState = ActionState.during;
        }
        else if (_roomWalker.AtDestination()) {
            CompleteAction();
        }
    }

    public void Search() {
        if (_actionState == ActionState.start) {
            _roomWalker.Hide();
            _actionState = ActionState.during;
        }
        else if (_roomWalker.AtDestination()) {
            CompleteAction();
        }
    }

    public void Hunt() {
        if (_actionState == ActionState.start) {
            if (_victim != null) {
                _roomWalker.EnterRoom(_victim._currentRoom);
            }
            else {
                Debug.LogWarning("Nemesis has no Victim to Hunt!");
            }
            _actionState = ActionState.during;
        }
        else if (_roomWalker.AtDestination()) {
            CompleteAction();
        }
    }

    private void DoSpooky(NemesisSpooky spooky) {
        _lastSpooky = spooky;
        switch (spooky) {
            case NemesisSpooky.wait:
                Wait();
                CompleteSpooky();
                break;
            case NemesisSpooky.jitter:
                Wait();
                CompleteSpooky();
                break;
            case NemesisSpooky.stare:
                Wait();
                CompleteSpooky();
                break;
        }
    }

    private void Wait() {
        _nextActionTime = Time.time + Random.Range(_minWait, _maxWait);
    }
}
