using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionState {
    start,
    during,
    complete
}

public class Nemesis : MonoBehaviour {

    public enum NemesisSpooky {
        wait,
        jitter,
        stare,
        spookyLimit
    }

    public enum NemesisAction {
        chase,
        explore,
        search,
        hunt,
        spooky,
        actionLimit
    }

    public RoomWalker _roomWalker;
    public RoomWalker _victim;

    public Room _startRoom;

    public ActionState _actionState;
    public NemesisAction _currentActionType;
    public NemesisSpooky _spooky;
    public float _nextActionTime;

    public float _minWait;
    public float _maxWait;

    public Transform _head;
    public Transform _watching;
    public RaycastHit _watchingRay;

    public Transform _chaseTarget;
    public float _roamSpeed;
    public float _chaseSpeed;

    public float _touchDistance;

	// Use this for initialization
	void Start () {
        _actionState = ActionState.complete;
        RoomCameraManager.RegisterNemisis(this);
        _roomWalker._currentRoom = _startRoom;
	}
	
	// Update is called once per frame
	void Update () {
        if (!_victim)
        {
            if (RoomCameraManager._professor)
            {
                _victim = RoomCameraManager._professor.GetComponent<RoomWalker>();
            }
        }
        Watch();
        if (_actionState == ActionState.complete) {
            float currentTime = Time.time;
            if (currentTime >= _nextActionTime) {
                Wait();
                _actionState = ActionState.start;
            }
        }
        else {
                switch (_currentActionType) {
                case NemesisAction.chase:
                    Chase();
                    break;
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
                    DoSpooky();
                    break;
                case NemesisAction.actionLimit:
                default:
                    Debug.LogError("Invalid NemesisAction![" + _currentActionType + "]");
                    break;
            }
        }
	}

    private void OnDrawGizmos() {
        if (_watching != null) {
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(_head.transform.position, _head.transform.position + TowardsWatch());

            if (_chaseTarget != null) {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(_head.transform.position, _watchingRay.point);
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        
        if (other.CompareTag("Professor")) {
            _watching = other.transform;
        }else if (other.CompareTag("Camera") && (_currentActionType == NemesisAction.explore || _currentActionType == NemesisAction.spooky) && _watching == null)
        {
            _watching = other.transform;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (_watching == other.transform)
        {
            _watching = null;
            FaceTowards(transform.forward);
        }
    }
    
    private void Watch() {
        if (_watching != null) {
            int layerMaskFunc = 1 << LayerMask.NameToLayer("Nemesis");
            if (_chaseTarget == null) {
                if (Physics.Raycast(_head.transform.position, TowardsWatch(), out _watchingRay, Mathf.Infinity, ~layerMaskFunc)) {
                    if (_watchingRay.collider.CompareTag("Professor")) {
                        BeginChase(_watchingRay.transform);
                    }
                }
            }
            else {
                if (Physics.Raycast(_head.transform.position, TowardsWatch(), out _watchingRay, _touchDistance, ~layerMaskFunc)) {
                    if (_watchingRay.collider.CompareTag("Professor")) {
                        Professor professor = _chaseTarget.gameObject.GetComponent<Professor>();
                        professor.TouchWithDeath();
                        StopChase();
                        CompleteAction();
                    }
                }
            }
        }
    }

    private Vector3 TowardsWatch() {
        Vector3 towards = _watching.transform.position - _head.transform.position;
        towards.Normalize();
        FaceTowards(towards);
        return towards;
    }

    private void FaceTowards(Vector3 dir)
    {
        _head.forward = dir;
    }

    private void CompleteAction() {
        _actionState = ActionState.complete;
        _spooky = ChooseSpooky();
        _currentActionType = NemesisAction.spooky;
    }

    private void CompleteSpooky() {
        _actionState = ActionState.complete;
        _currentActionType = (NemesisAction)Random.Range((int)NemesisAction.explore, (int)NemesisAction.actionLimit);
    }

    private void BeginChase(Transform xform) {
        _chaseTarget = xform;
        _roomWalker._agent.speed = _chaseSpeed;
        _currentActionType = NemesisAction.chase;
        _actionState = ActionState.during;
    }

    private void StopChase() {
        _chaseTarget = null;
        _roomWalker._agent.speed = _roamSpeed;
        CompleteAction();
    }

    private void Chase() {
        if (_actionState == ActionState.start ||
            _actionState == ActionState.during) {
            if (_watching == null) {
                StopChase();
                _currentActionType = NemesisAction.search;
            }
            else if (_chaseTarget == null) {
                Debug.Log("Lost the target!");
                StopChase();
                CompleteSpooky();
            }
            else if (_roomWalker._agent.remainingDistance <= _roomWalker._agent.stoppingDistance) {
                StopChase();
                CompleteSpooky();
            }
            else {
                _roomWalker.MoveTowards(_chaseTarget.position);
            }
        }
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
            int random = Random.Range(0, 1);
            switch (random) {
                case 0:
                    _roomWalker.Hide();
                    break;
                case 1:
                    _roomWalker.Wander();
                    break;
            }
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

    private void DoSpooky() {
        switch (_spooky) {
            case NemesisSpooky.wait:
                Wait();
                CompleteSpooky();
                break;
            case NemesisSpooky.jitter:
                Wait();
                CompleteSpooky();
                break;
           case NemesisSpooky.stare:
                /*CameraController c = _roomWalker._currentRoom.GetRandomCamera();
                Vector3 cameraDirect = c.transform.position - _head.transform.position;
                FaceTowards(cameraDirect);*/
                Wait();
                CompleteSpooky();
                break;
        }
    }

    private NemesisSpooky ChooseSpooky() {
        NemesisSpooky spooky = (NemesisSpooky)Random.Range((int)NemesisAction.explore, (int)NemesisSpooky.spookyLimit);
        //while (spooky != _spooky) { // don't do the same spooky twice
        //    spooky = (NemesisSpooky)Random.Range((int)NemesisAction.explore, (int)NemesisSpooky.spookyLimit);
        //}
        return spooky;
    }



private void Wait() {
        _nextActionTime = Time.time + Random.Range(_minWait, _maxWait);
    }
}
