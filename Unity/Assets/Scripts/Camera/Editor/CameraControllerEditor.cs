using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CameraController))]
public class CameraControllerEditor : Editor {

    private Quaternion _originalQuat;
    private bool _isTesting;
    float yaw = 0;

    private RenderTexture _lastTexture;

    public void OnEnable()
    {
        _lastTexture = new RenderTexture(1280, 720, 16);
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        CameraController c = (CameraController)target;
        if(c._passiveCamera)
        c._passiveCamera.targetTexture = _lastTexture;

        EditorGUILayout.Space();
        

        yaw = EditorGUILayout.Slider("Inital Yaw", yaw, 0, 1);
        _isTesting = EditorGUILayout.Toggle("Test Camera", _isTesting);

        if (_isTesting)
        {
            c.transform.rotation = Quaternion.Slerp(_originalQuat * Quaternion.Euler(0, c._minYaw, 0), _originalQuat * Quaternion.Euler(0, c._maxYaw, 0), yaw);
        }
        else
        {
            _originalQuat = c.transform.rotation;
        }
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
