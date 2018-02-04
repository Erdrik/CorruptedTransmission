using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CustomHandles
{

    public enum MouseResult
    {
        None = 0,

        LMBPress,
        LMBClick,
        LMBDoubleClick,
        LMBDrag,
        LMBRelease,

        RMBPress,
        RMBClick,
        RMBDoubleClick,
        RMBDrag,
        RMBRelease
    }

    static int _Move2DHandleHash = "MoveDiscHash".GetHashCode();
    static Vector2 _Move2DHandleMouseStart;
    static Vector2 _Move2DHandleMouseCurrent;
    static Vector3 _Move2DHandleWorldStart;
    static float _Move2DHandleClickTime = 0;
    static int _Move2DHandleClickID;
    static float _Move2DHandleDoubleClickInterval = 0.5f;
    static bool _Move2DHandleHasMoved;

    public static int _lastMove2DHandleID;

    public static Vector3 Move2DHandle(Vector3 position, float handleSize, Vector3 normal, Handles.CapFunction capFunc, Color colorHovered, Color colorSelected, out MouseResult result, float snap = -1, bool snapLocal = false)
    {
        int id = GUIUtility.GetControlID(_Move2DHandleHash, FocusType.Passive);
        _lastMove2DHandleID = id;

        Vector3 screenPosition = Handles.matrix.MultiplyPoint(position);
        Matrix4x4 cachedMatrix = Handles.matrix;

        result = MouseResult.None;

        switch (Event.current.GetTypeForControl(id))
        {
            case EventType.MouseDown:
                result = Move2DHandleMouseDown(id, position);
                break;
            case EventType.MouseUp:
                result = Move2DHandleMouseUp(id, ref position, snap, snapLocal);
                break;
            case EventType.MouseDrag:
                result = Move2DHandleMouseDrag(id, ref position, normal);
                break;
            case EventType.Repaint:
                Color currentColour = Handles.color;
                if (HandleUtility.nearestControl == id && id != GUIUtility.hotControl)
                {
                    Handles.color = colorHovered;
                }
                else if (id == GUIUtility.hotControl && _Move2DHandleHasMoved)
                {
                    Handles.color = colorSelected;
                }

                Handles.matrix = Matrix4x4.identity;
                capFunc(id, screenPosition, Quaternion.Euler(90,0,0), handleSize, EventType.Repaint);
                Handles.matrix = cachedMatrix;

                Handles.color = currentColour;
                break;

            case EventType.Layout:
                Handles.matrix = Matrix4x4.identity;
                HandleUtility.AddControl(id, HandleUtility.DistanceToCircle(screenPosition, handleSize));
                Handles.matrix = cachedMatrix;
                break;
        }
        return position;
    }

    private static MouseResult Move2DHandleMouseDown(int id, Vector3 position)
    {
        if (HandleUtility.nearestControl == id && (Event.current.button == 0 || Event.current.button == 1))
        {
            GUIUtility.hotControl = id;
            _Move2DHandleMouseCurrent = _Move2DHandleMouseStart = Event.current.mousePosition;
            _Move2DHandleWorldStart = position;
            _Move2DHandleHasMoved = false;

            Event.current.Use();
            //EditorGUIUtility.SetWantsMouseJumping(1);

            if (Event.current.button == 0)
            {
                return MouseResult.LMBPress;
            }
            else if (Event.current.button == 1)
            {
                return MouseResult.RMBPress;
            }
        }
        return MouseResult.None;
    }

    private static MouseResult Move2DHandleMouseUp(int id, ref Vector3 position, float snap, bool localSnap = false)
    {
        MouseResult result = MouseResult.None;
        if (GUIUtility.hotControl == id && (Event.current.button == 0 || Event.current.button == 1))
        {
            GUIUtility.hotControl = 0;
            Event.current.Use();
            //EditorGUIUtility.SetWantsMouseJumping(0);
            if (snap > 0) {
                if (!localSnap)
                {
                    position = new Vector3(Mathf.Round(position.x / snap) * snap, Mathf.Round(position.y / snap) * snap, Mathf.Round(position.z / snap) * snap);
                }
                else
                {
                    Debug.Log(new Vector3(position.x % snap, position.y % snap, position.z % snap));
                    position = new Vector3(position.x%snap,position.y%snap,position.z%snap) + new Vector3(Mathf.Round(position.x / snap) * snap, Mathf.Round(position.y / snap) * snap, Mathf.Round(position.z / snap) * snap);
                }
            }
                
            if (Event.current.button == 0)
                result = MouseResult.LMBRelease;
            else if (Event.current.button == 1)
                result = MouseResult.RMBRelease;

            if (Event.current.mousePosition == _Move2DHandleMouseStart)
            {
                bool doubleClick = (_Move2DHandleClickID == id) && (Time.realtimeSinceStartup - _Move2DHandleClickTime < _Move2DHandleDoubleClickInterval);

                _Move2DHandleClickID = id;
                _Move2DHandleClickTime = Time.realtimeSinceStartup;

                if (Event.current.button == 0)
                    result = doubleClick ? MouseResult.LMBDoubleClick : MouseResult.LMBClick;
                else if (Event.current.button == 1)
                    result = doubleClick ? MouseResult.RMBDoubleClick : MouseResult.RMBClick;
            }
        }
        return result;
    }

    private static MouseResult Move2DHandleMouseDrag(int id, ref Vector3 position, Vector3 normal)
    {
        MouseResult result = MouseResult.None;
        if (GUIUtility.hotControl == id)
        {
            _Move2DHandleMouseCurrent += new Vector2(Event.current.delta.x, -Event.current.delta.y);
            Vector3 position2 = Camera.current.WorldToScreenPoint(Handles.matrix.MultiplyPoint(_Move2DHandleWorldStart))
                     + (Vector3)(_Move2DHandleMouseCurrent - _Move2DHandleMouseStart);
            position = Handles.matrix.inverse.MultiplyPoint(Camera.current.ScreenToWorldPoint(position2));

            if (Camera.current.transform.forward == Vector3.forward || Camera.current.transform.forward == -Vector3.forward)
                position.z = _Move2DHandleWorldStart.z;
            if (Camera.current.transform.forward == Vector3.up || Camera.current.transform.forward == -Vector3.up)
                position.y = _Move2DHandleWorldStart.y;
            if (Camera.current.transform.forward == Vector3.right || Camera.current.transform.forward == -Vector3.right)
                position.x = _Move2DHandleWorldStart.x;

            if (Event.current.button == 0)
                result = MouseResult.LMBDrag;
            else if (Event.current.button == 1)
                result = MouseResult.RMBDrag;

            _Move2DHandleHasMoved = true;

            GUI.changed = true;
            Event.current.Use();

        }
        return result;
    }




    static int _SelectLineHandleHash = "SelectLineHandleHash".GetHashCode();
    static bool _SelectLineHandleIsHovered;

    public static int _lastSelectLineHandleID;

    public static Vector3 SelectLineHandle(Vector3 startPositon, Vector3 endPosition, float handleThickness, Color colorHovered, Color colorSelected, out MouseResult result)
    {
        int id = GUIUtility.GetControlID(_SelectLineHandleHash, FocusType.Passive);
        _lastSelectLineHandleID = id;

        Matrix4x4 cachedMatrix = Handles.matrix;

        Vector3 screenStartPos = Handles.matrix.MultiplyPoint(startPositon);
        Vector3 screenEndPos = Handles.matrix.MultiplyPoint(endPosition);
        Vector3 screenUnitVector = (startPositon - endPosition).normalized;
        float length = (startPositon - endPosition).magnitude;
        float unit = (length / handleThickness);

        result = MouseResult.None;
        Vector3 selectedPoint = Vector2.zero;

        switch (Event.current.GetTypeForControl(id))
        {
            case EventType.MouseDown:
                result = SelectLineHandleMouseDown(id,startPositon,endPosition, out selectedPoint);
                break;
            case EventType.MouseUp:
                //result = Move2DHandleMouseUp(id, position);
                break;
            case EventType.MouseDrag:
                //result = Move2DHandleMouseDrag(id, ref position, normal);
                break;
            case EventType.Repaint:
                Color currentColour = Handles.color;

                _SelectLineHandleIsHovered = (id == HandleUtility.nearestControl);

                if (GUIUtility.hotControl == id && (Event.current.button == 0 || Event.current.button == 1))
                {
                    Handles.color = colorSelected;
                }
                else if (_SelectLineHandleIsHovered)
                {
                    Handles.color = colorHovered;
                }

                Handles.matrix = Matrix4x4.identity;
                Handles.DrawAAPolyLine(handleThickness, startPositon, endPosition);
                Handles.matrix = cachedMatrix;

                Handles.color = currentColour;
                break;

            case EventType.Layout:
                Handles.matrix = Matrix4x4.identity;

                HandleUtility.AddControl(id, HandleUtility.DistanceToLine(screenStartPos, screenEndPos));
                Handles.matrix = cachedMatrix;
                break;
        }
        return selectedPoint;
    }

    private static MouseResult SelectLineHandleMouseDown(int id, Vector3 startPosition, Vector3 endPosition, out Vector3 linePoint)
    {
        linePoint = Vector2.zero;
        if (HandleUtility.nearestControl == id && (Event.current.button == 0 || Event.current.button == 1))
        {
            Vector3 midPoint = Vector3.Lerp(startPosition, endPosition, 0.5f);
            linePoint = midPoint;

            Event.current.Use();
            //EditorGUIUtility.SetWantsMouseJumping(1);

            if (Event.current.button == 0)
            {
                Debug.Log("click");
                return MouseResult.LMBPress;
            }
            else if (Event.current.button == 1)
            {
                return MouseResult.RMBPress;
            }
        }
        return MouseResult.None;
    }


}