using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CameraController))]
public class CameraControllerEditor : Editor {

    private Quaternion _originalQuat;
    private bool _isTesting;
    private bool _wasTesting;
    float yaw = 0;

    private RenderTexture _lastTexture;
    private CameraController _camera;

    public void OnEnable()
    {
        _lastTexture = new RenderTexture(1280, 720, 16);
    }

    public void OnDisable()
    {
        _camera.transform.rotation = _originalQuat;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        _camera = (CameraController)target;
        if(_camera._passiveCamera)
            _camera._passiveCamera.targetTexture = _lastTexture;

        EditorGUILayout.Space();
        

        yaw = EditorGUILayout.Slider("Inital Yaw", yaw, 0, 1);
        _isTesting = EditorGUILayout.Toggle("Test Camera", _isTesting);

        if (_isTesting)
        {
            _camera.transform.rotation = Quaternion.Slerp(_originalQuat * Quaternion.Euler(0, _camera._minYaw, 0), _originalQuat * Quaternion.Euler(0, _camera._maxYaw, 0), yaw);
        }else if (!_isTesting && _wasTesting)
        {
            _camera.transform.rotation = _originalQuat;
        }
        else
        {
            _originalQuat = _camera.transform.rotation;
        }
        _wasTesting = _isTesting;
    }

    public override bool HasPreviewGUI()
    {
        return true;
    }

    public override void OnPreviewGUI(Rect r, GUIStyle background)
    {
        if (_lastTexture) {
            GUI.DrawTexture(r, _lastTexture);
        }
    }


}
