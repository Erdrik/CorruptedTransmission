using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraUISelectionManager : MonoBehaviour {

    public CameraFeed _feedPrefab;
    public Room _activeRoom;
    public RectTransform _root;

    public Dictionary<CameraController,CameraFeed> _activeFeeds = new Dictionary<CameraController,CameraFeed>();

    private void Update()
    {
        CheckCameras();
    }

    public void SetActiveRoom(Room room)
    {
        ClearTextures();
        _activeRoom = room;
        foreach(CameraController camera in room._cameraPoints)
        {
            CreateNewTexture(camera);
        }
        CheckCameras();
    }

    private void CheckCameras()
    {
        if (_activeRoom)
        {
            foreach (CameraController camera in _activeRoom._cameraPoints)
            {
                if (_activeFeeds.ContainsKey(camera))
                {
                    if (camera._cameraState != CameraController.CameraState.Disabled)
                    {
                        _activeFeeds[camera].EnableTexture();
                    }
                    else
                    {
                        _activeFeeds[camera].DisableTexture();
                    }
                }
            }
        }
    }

    private void CreateNewTexture(CameraController camera)
    {
        CameraFeed newFeed = Instantiate(_feedPrefab);
        _activeFeeds.Add(camera,newFeed);
        newFeed.GetComponent<RectTransform>().SetParent(_root, false);
        newFeed.SetTexture(camera._passiveCamera.targetTexture);
        newFeed.Init(camera);
    }

    private void ClearTextures()
    {
        foreach (var c in _activeFeeds)
        {
            Destroy(c.Value);
        }
        _activeFeeds.Clear();
    }
	
}
