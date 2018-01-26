using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(RoomCameraManager))]
[CanEditMultipleObjects]
public class RoomCameraManager_Editor : Editor {

    private SerializedProperty _cameras;
    private SerializedProperty _currentCamera;

    void OnEnable() {
        _cameras = serializedObject.FindProperty(RoomCameraManager.PROPERTY_CAMERAS);
        _currentCamera = serializedObject.FindProperty(RoomCameraManager.PROPERTY_CURRENT_CAMERA);
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();
        
        EditorGUILayout.PropertyField(_cameras, new GUIContent("Cameras"), true);

        int cameraIndex = EditorGUILayout.DelayedIntField(new GUIContent("Current Camera"), _currentCamera.intValue);
        if (cameraIndex > 0 &&
            cameraIndex < _cameras.arraySize) {
            _currentCamera.intValue = cameraIndex;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
