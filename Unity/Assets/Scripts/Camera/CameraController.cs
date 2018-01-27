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

    public AudioSource _audioSource;
    public Camera _passiveCamera;

    [Range(-360,0)]
    public float _minPitch = -45;
    [Range(0,360)]
    public float _maxPitch = 45;

    [Range(0,1)]
    public float _currentYaw = 0;

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

    public float _delay = 1f;

    public bool _isRoaming = false;

    public CameraState _cameraState = CameraState.NotWatched;
    private Quaternion _originalOrientation;

    private RenderTexture _cameraTexture;
    public Room _owningRoom;

    // Use this for initialization
    void Start () {
        _originalOrientation = transform.rotation;
        _cameraTexture = new RenderTexture(300, 168, 16, RenderTextureFormat.ARGB64);
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
        if (!_isRoaming)
        {
            _isRoaming = true;
            StartCoroutine(RoamCamera());
            
        }
    }

    void CreateRenderTexture()
    {
        
        _cameraTexture.Create();
        

    }

    void ReleaseRenderTexture()
    {
        _cameraTexture.Release();
    }

    IEnumerator RoamCamera()
    {
        while (_isRoaming && _yawRoam)
        {
            Debug.Log("start");
            float currentPoint = _minYaw;
            _audioSource.Play();
            while (currentPoint <= _maxYaw)
            {
                currentPoint += _yawRoamSpeed * Time.deltaTime;
                transform.rotation = _originalOrientation * Quaternion.Euler(0, currentPoint, 0);
                yield return new WaitForEndOfFrame();
            }
            transform.rotation = _originalOrientation * Quaternion.Euler(0, _maxYaw, 0);
            _audioSource.Stop();
            yield return new WaitForSeconds(_delay);
            _audioSource.Play();
            while (currentPoint >= _minYaw)
            {
                currentPoint -= _yawRoamSpeed * Time.deltaTime;
                transform.rotation = _originalOrientation * Quaternion.Euler(0, currentPoint, 0);
                yield return new WaitForEndOfFrame();
            }
            transform.rotation = _originalOrientation * Quaternion.Euler(0, _minYaw, 0);
            _audioSource.Stop();
            yield return new WaitForSeconds(_delay);
        }
    }

    private void OnDrawGizmos()
    {
        
        Quaternion quat = Application.isPlaying ? _originalOrientation : transform.rotation;
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, quat * Quaternion.Euler(0, _minYaw, 0) * (Vector3.forward * 5));
        Gizmos.DrawRay(transform.position, quat * Quaternion.Euler(0, _maxYaw, 0) * (Vector3.forward * 5));
        Gizmos.DrawLine(transform.position + quat * Quaternion.Euler(0, _minYaw, 0) * (Vector3.forward * 5), transform.position + quat * Quaternion.Euler(0, _maxYaw, 0) * (Vector3.forward * 5));
        //Gizmos.DrawLine(transform.position, transform.rotation * Quaternion.Euler(0, _maxYaw, 0) * (Vector3.forward*5));
    }

}
