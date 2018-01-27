using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public float _minPitch = -45;
    public float _maxPitch = 45;

    public float _minYaw = -45;
    public float _maxYaw = 45;

    public float _pitchSpeed = 5;
    public float _yawSpeed = 5;

    public bool _pitchRoam = true;
    public bool _yawRoam;

    public float _pitchRoamSpeed = 5;
    public float _yawRoamSpeed = 5;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void Roam()
    {

    }


}
