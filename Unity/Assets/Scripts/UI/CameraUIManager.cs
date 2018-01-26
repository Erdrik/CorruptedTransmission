using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraUIManager : MonoBehaviour {

    public RectTransform _rootPoint;
    public float _cameraSpacing = 20;
    public float _cameraSize = 60;

    public CameraManager _cameraManager;
    public GameObject _CameraUIPrefab;

    public void Start()
    {
        BuildCameraUI();
    }

    public void CycleCameraForward()
    {
        _cameraManager.CycleCameraForward();
    }

    public void CycleCameraBackwards()
    {
        _cameraManager.CycleCameraBackward();
    }

    public void BuildCameraUI()
    {
        CameraObject camera = CameraObject._rootCamera;

        if (camera)
        {
            Debug.Log(camera);
            FindLastNode(camera);
        }
    }
    private void FindLastNode(CameraObject node)
    {
        FindLastNode(node, Vector2Int.zero, new HashSet<CameraObject>());
    }

    private void FindLastNode(CameraObject node, Vector2Int position, HashSet<CameraObject> doneRooms)
    {
        if (!doneRooms.Contains(node))
        {
            doneRooms.Add(node);
            List<CameraObject.CameraDirection> neighbours = node._cameraNeighbours;
            foreach (CameraObject.CameraDirection n in neighbours)
            {

                switch (n._cameraDirecion)
                {
                    case Direction.Forward:
                        FindLastNode(n._camera, position + new Vector2Int(0, 1), doneRooms);
                        break;
                    case Direction.Backward:
                        FindLastNode(n._camera, position + new Vector2Int(0, -1), doneRooms);
                        break;
                    case Direction.Left:
                        FindLastNode(n._camera, position + new Vector2Int(-1, 0), doneRooms);
                        break;
                    case Direction.Right:
                        FindLastNode(n._camera, position + new Vector2Int(1, 0), doneRooms);
                        break;
                    default:
                        break;
                }

            }
            BuildCameraUIPoint(position, 0);
        }
    }

    

    private void BuildCameraUIPoint(Vector2Int position, int i)
    {
        GameObject cameraPoint = Instantiate(_CameraUIPrefab);
        RectTransform cameraPointTransform = cameraPoint.GetComponent<RectTransform>();
        cameraPointTransform.SetParent(GetComponent<RectTransform>(), false);
        cameraPointTransform.anchoredPosition3D = new Vector2(position.x, position.y) * (_cameraSpacing + _cameraSize);
        //cameraPointTransform.position = new Vector3(cameraPointTransform.transform.position.x, cameraPointTransform.transform.position.y, 0);
    }

    

}
