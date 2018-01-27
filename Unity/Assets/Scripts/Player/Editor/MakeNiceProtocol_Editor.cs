using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(MakeNiceProtocol))]
[CanEditMultipleObjects]
public class MakeNiceProtocolEditor : Editor {

    private SerializedProperty _professor;
    private SerializedProperty _roomCameraManager;

    private int _selectedRoom;

    void OnEnable() {
        _professor = serializedObject.FindProperty(MakeNiceProtocol.PROPERTY_PROFESSOR);
        _roomCameraManager = serializedObject.FindProperty(MakeNiceProtocol.PROPERTY_ROOM_CAMERA_MANAGER);
        _selectedRoom = 0;
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();

        EditorGUILayout.PropertyField(_professor, new GUIContent("Professor"));
        EditorGUILayout.PropertyField(_roomCameraManager, new GUIContent("Room Camera Manager"));

        MakeNiceProtocol makeNice = (MakeNiceProtocol) serializedObject.targetObject;

        if (GUILayout.Button(new GUIContent("Move"))) {
            makeNice.InstructMove();
        }

        EditorGUILayout.BeginHorizontal();
        _selectedRoom = EditorGUILayout.IntField(new GUIContent("Selected Room"), _selectedRoom);
        if (GUILayout.Button(new GUIContent("Room"))) {
            makeNice.InstructRoom(_selectedRoom);
        }
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button(new GUIContent("Hide"))) {
            makeNice.InstructHide();
        }

        if (GUILayout.Button(new GUIContent("Get Out"))) {
            makeNice.InstructGetOut();
        }

        if (GUILayout.Button(new GUIContent("Go Back"))) {
            makeNice.InstructGoBack();
        }

        if (GUILayout.Button(new GUIContent("Stop"))) {
            makeNice.InstructStop();
        }

        if (GUILayout.Button(new GUIContent("Push Button"))) {
            makeNice.InstructPushButton();
        }

        serializedObject.ApplyModifiedProperties();
    }
}
