using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

    public Transform _door;

    public Vector3 _closed;
    public Vector3 _open;

    public float _timeTaken;
    public float _startTime;

    public bool _locking;

    [SerializeField]
    [HideInInspector]
    private bool _locked;

	// Use this for initialization
	void Start () {
        _locked = _locking;
        _startTime = 0.0f - _timeTaken;
	}
	
	// Update is called once per frame
	void Update () {
        if (_locking != _locked) {
            _locked = _locking;
            _startTime = Time.time;
        }
        else {
            float weight = (Time.time - _startTime) / _timeTaken;
            if (weight >= 1.0f) {
                // Do nothing
            }
            else if (_locked) {
                _door.localPosition = Vector3.Lerp(_open, _closed, weight);
            }
            else {
                _door.localPosition = Vector3.Lerp(_closed, _open, weight);
            }
        }
    }

    public void Lock() {
        _locking = true;
    }

    public void Unlock() {
        _locking = false;
    }
}
