using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraUISelectionManager : MonoBehaviour {

    public RectTransform _feedPrefab;
    public Room _activeRoom;
    public RectTransform _root;

    public List<Image> _activeFeeds = new List<Image>();

    public void SetActiveRoom(Room room)
    {
        ClearTextures();
        _activeRoom = room;
        foreach(CameraController camera in room._cameraPoints)
        {
            CreateNewTexture(camera);
        }
    }

    private void CreateNewTexture(CameraController camera)
    {
        RectTransform newFeed = Instantiate(_feedPrefab);
        newFeed.SetParent(_root, false);
        newFeed.GetComponentInChildren<RawImage>().material = new Material(newFeed.GetComponentInChildren<RawImage>().material);
        newFeed.GetComponentInChildren<RawImage>().material.mainTexture = camera._passiveCamera.targetTexture;
        newFeed.GetComponent<Button>().onClick.AddListener(() =>
        {
            RoomCameraManager.GoToCamera(camera);
        });
    }

    private void ClearTextures()
    {
        for (int i = 0; i < _activeFeeds.Count; i++)
        {
            Destroy(_activeFeeds[i]);
            _activeFeeds.RemoveAt(i);
        }
    }
	
}
