using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSway : MonoBehaviour {

    public float _xSwayAmount = 0.5f;
    public float _ySwayAmount = 0.5f;

    public Transform _cameraTransform;
    public bool _sway = true;

    private Vector3 _orignalPos;

    private void Start()
    {
        _orignalPos = _cameraTransform.localPosition;
    }

    // Update is called once per frame
    void Update () {
        if (_sway)
        {
            Vector2 realPos = Vector2.ClampMagnitude(new Vector2(((Input.mousePosition.x / Screen.width) * 2) - 1, ((Input.mousePosition.y / Screen.height) * 2) - 1), 1);
            Vector3 swayedPosition = _orignalPos + _cameraTransform.right * _xSwayAmount * realPos.x + _cameraTransform.up * _ySwayAmount * realPos.y;
            _cameraTransform.localPosition = swayedPosition;
        }
	}
}
