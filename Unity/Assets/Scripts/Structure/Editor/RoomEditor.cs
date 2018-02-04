using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Room))]
public class RoomEditor : Editor {

    const float snapTime = 0.1f;

    SerializedProperty _roomBounds;

    float snapAngle = -1;
    float snapDistance = -1;
    bool snapAngleToggle = false;
    bool snapDistanceToggle = false;
    bool snapLocal = false;

    private void OnEnable()
    {
        _roomBounds = serializedObject.FindProperty(Room.PROPERTY_ROOMS_BOUNDS);
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        snapAngleToggle = EditorGUILayout.BeginToggleGroup("Snap Angle", snapAngleToggle);
        if (snapAngleToggle)
        {
            snapAngle = Mathf.Clamp(EditorGUILayout.FloatField("Snap Angle", snapAngle),0,90);
        }
        else
        {
            snapAngle = -1;
        }
        EditorGUILayout.EndToggleGroup();

        snapDistanceToggle = EditorGUILayout.BeginToggleGroup("Snap Position", snapDistanceToggle);
        if (snapDistanceToggle)
        {
            snapDistance = Mathf.Clamp(EditorGUILayout.FloatField("Snap Distance", snapDistance),0,1000);
            snapLocal = EditorGUILayout.ToggleLeft("Local Snap", snapLocal);
        }
        else
        {
            snapDistance = -1;
        }
        EditorGUILayout.EndToggleGroup();

        SerializedProperty points = _roomBounds.FindPropertyRelative(Polygon2D.PROPERTY_POINTS);

        if (GUILayout.Button("Localise", EditorStyles.miniButton)){
            for (int i = 0; i < points.arraySize; i++)
            {
                Transform pos = ((Room)serializedObject.targetObject).transform;
                SerializedProperty pointPtr = points.GetArrayElementAtIndex(i);
                Vector3 relativePos = pos.InverseTransformPoint(new Vector3(pointPtr.vector2Value.x, pos.position.y, pointPtr.vector2Value.y));
                pointPtr.vector2Value = new Vector2(relativePos.x, relativePos.z);
            }
            serializedObject.ApplyModifiedProperties();
        }

    }

    private void OnSceneGUI()
    {
        SerializedProperty points = _roomBounds.FindPropertyRelative(Polygon2D.PROPERTY_POINTS);

        for (int i = 0; i < points.arraySize-1; i++)
        {
            HandleLine(i, i+1, points);
        }

        HandleLine(points.arraySize - 1, 0, points);

        for (int i = 0; i < points.arraySize-1; i++)
        {
            HandlePoint(i, i + 1, points);
        }

        HandlePoint(points.arraySize - 1, 0, points);


        serializedObject.ApplyModifiedProperties();
    }

    void HandleLine(int i, int nt, SerializedProperty points)
    {
        Vector3 pos = ((Room)serializedObject.targetObject).transform.position;
        SerializedProperty pointPtr = points.GetArrayElementAtIndex(i);
        SerializedProperty pointNextPtr = points.GetArrayElementAtIndex(nt);

        CustomHandles.MouseResult result;

        Vector3 point = new Vector3(pointPtr.vector2Value.x, 0, pointPtr.vector2Value.y)+pos;
        Vector3 newPoint = new Vector3(pointNextPtr.vector2Value.x, 0, pointNextPtr.vector2Value.y)+pos;
 
        Vector3 v = CustomHandles.SelectLineHandle(point, newPoint, 5f, Color.cyan, Color.red, out result);

        if (result == CustomHandles.MouseResult.LMBPress)
        {
            Debug.Log("hit");
            AddNewPoint(i, v);
            serializedObject.ApplyModifiedProperties();
            HandleUtility.Repaint();
        }
    }

    void HandlePoint(int i, int ni, SerializedProperty points)
    {
        Vector3 pos = ((Room)serializedObject.targetObject).transform.position;
        CustomHandles.MouseResult result;
        SerializedProperty pointPtr = points.GetArrayElementAtIndex(ni);
        SerializedProperty lastPointPtr = points.GetArrayElementAtIndex(i);
        Vector3 point = new Vector3(pointPtr.vector2Value.x, 0, pointPtr.vector2Value.y)+pos;

        Handles.matrix = Matrix4x4.identity;
        point = CustomHandles.Move2DHandle(point, 0.3f * HandleUtility.GetHandleSize(point), Vector3.up, Handles.CircleHandleCap, Color.cyan, Color.red, out result, snapDistance, snapLocal);

        pointPtr.vector2Value = new Vector2(point.x, point.z) - new Vector2(pos.x, pos.z);

        if (result == CustomHandles.MouseResult.RMBPress)
        {
            RemovePoint(ni);
            serializedObject.ApplyModifiedProperties();
            HandleUtility.Repaint();
            return;
        }

        if (result == CustomHandles.MouseResult.LMBRelease)
        {
            if (snapAngle > 0)
            {
                Vector2 direction = lastPointPtr.vector2Value - pointPtr.vector2Value;
                float length = direction.magnitude;
                float angle = Vector2.SignedAngle(Vector2.up, direction);
                Debug.Log(length);
                Vector2 newDirection = Quaternion.Euler(0, 0, Mathf.Round(angle / snapAngle) * snapAngle) * Vector2.down;
                direction = lastPointPtr.vector2Value + (newDirection * length);
                pointPtr.vector2Value = direction;
                serializedObject.ApplyModifiedProperties();
                HandleUtility.Repaint();
                return;
            }
            //pointPtr.vector2Value
        }
        serializedObject.ApplyModifiedProperties();
    }

    void RemovePoint(int i)
    {
        SerializedProperty points = _roomBounds.FindPropertyRelative(Polygon2D.PROPERTY_POINTS);
        points.DeleteArrayElementAtIndex(i);
    }

    void AddNewPoint(int i, Vector3 point)
    {
        Vector3 pos = ((Room)serializedObject.targetObject).transform.position;
        Debug.Log(point);
        SerializedProperty points = _roomBounds.FindPropertyRelative(Polygon2D.PROPERTY_POINTS);
        points.InsertArrayElementAtIndex(i+1);
        points.GetArrayElementAtIndex(i+1).vector2Value = new Vector3(point.x, point.z) - new Vector3(pos.x, pos.z);
    }

}
