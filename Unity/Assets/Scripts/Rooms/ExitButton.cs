using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitButton : MonoBehaviour {

    public ExitDoor _exit;

    [Range(0, 2)]
    public int _id;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Professor") && 
            !_exit.IsActivated(_id)) {
            _exit.Activate(_id);
        }
    }

    public bool Activated {
        get {
            return _exit.IsActivated(_id);
        }
    }
}
