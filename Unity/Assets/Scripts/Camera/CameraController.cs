using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public enum CameraState
    {
        NotWatched,
        InRoom,
        Watched,
        Controlled,
        Disabled
    }

    public Camera _passiveCamera;

    [Range(-360,0)]
    public float _minPitch = -45;
    [Range(0,360)]
    public float _maxPitch = 45;

    [Range(-360, 0)]
    public float _minYaw = -45;
    [Range(0, 360)]
    public float _maxYaw = 45;

    public float _pitchSpeed = 5;
    public float _yawSpeed = 5;

    public bool _pitchRoam = true;
    public bool _yawRoam;

    public float _pitchRoamSpeed = 5;
    public float _yawRoamSpeed = 5;

    public CameraState _cameraState = CameraState.NotWatched;
    private Quaternion _originalOrientation;

    private RenderTexture _cameraTexture;
    public Room _owningRoom;

    // Use this for initialization
    void Start () {
        _originalOrientation = transform.rotation;
        _cameraTexture = new RenderTexture(300, 168, 0);
        _passiveCamera.targetTexture = _cameraTexture;
    }
	
	// Update is called once per frame
	void Update () {
        switch (_cameraState)
        {
            case CameraState.NotWatched:
                NotWatchedState();
                break;
            case CameraState.Watched:
                WatchedState();
                break;
            case CameraState.InRoom:
                InRoomState();
                break;
            case CameraState.Controlled:
                ControlledState();
                break;
            case CameraState.Disabled:
                DisabledState();
                break;
            default:
                break;
        }
    }

    public void SetRoom(Room r)
    {
        _owningRoom = r;
    }

    private void ChangeState(CameraState _newState)
    {
        if(_newState == CameraState.InRoom)
        {
            CreateRenderTexture();
        }
        if (_newState != CameraState.InRoom)
        {
            ReleaseRenderTexture();
        }
        _cameraState = _newState;
    }

    private void NotWatchedState()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            ChangeState(CameraState.InRoom);
        }
        Roam();
    }

    private void WatchedState()
    {
        Roam();
    }

    private void InRoomState()
    {

        Roam();
    }

    private void ControlledState()
    {

    }

    private void DisabledState()
    {

    }

    void Roam()
    {
        transform.rotation = _originalOrientation * Quaternion.Euler(0,Mathf.PingPong(Time.time*10, (_maxYaw)-_minYaw)+_minYaw,0);
    }

    void CreateRenderTexture()
    {
        
        _cameraTexture.Create();
        

    }

    void ReleaseRenderTexture()
    {
        _cameraTexture.Release();
    }




}
