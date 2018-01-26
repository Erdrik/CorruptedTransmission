using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {

    public List<Transform> _hidingPoints;
    public List<Transform> _stopPoints;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public Vector3 GetRandomPoint(List<Transform> xforms) {
        return xforms[Random.Range(0, xforms.Count - 1)].position;
    }

    public Vector3 GetRandomStopPoint() {
        return GetRandomPoint(_stopPoints);
    }

    public Vector3 GetRandomHidingPoint() {
        return GetRandomPoint(_hidingPoints);
    }
}
