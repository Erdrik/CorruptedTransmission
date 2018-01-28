using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusCamera : MonoBehaviour {

    private Vector3 orignalPoint;
    private Quaternion orignalQuat;

    private FocusCamera _instance;

	// Use this for initialization
	void Start () {
        _instance = this;
        orignalPoint = transform.position;
        orignalQuat = transform.rotation;
	}

    static void GoToFocusPoint()
    {
        
    }
}
