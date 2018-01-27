using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraUISelectionManager : MonoBehaviour {

    public RectTransform _feedPrefab;
    public Room _activeRoom;
    public RectTransform _root;

    public List<RectTransform> _activeFeeds = new List<RectTransform>();

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
        _activeFeeds.Add(newFeed);
        newFeed.SetParent(_root, false);
        newFeed.GetComponentInChildren<RawImage>().material = new Material(newFeed.GetComponentInChildren<RawImage>().material);
        newFeed.GetComponentInChildren<RawImage>().material.mainTexture = camera._passiveCamera.targetTexture;
        newFeed.GetComponent<Button>().onClick.AddListener(() =>
        {
            RoomCameraManager.GoToCamera(camera);
            newFeed.GetComponent<Button>().OnDeselect(new UnityEngine.EventSystems.BaseEventData(UnityEngine.EventSystems.EventSystem.current));
        });
    }

    private void ClearTextures()
    {
        for (int i = 0; i < _activeFeeds.Count; i++)
        {
            Destroy(_activeFeeds[i].gameObject);
            
        }
        _activeFeeds.Clear();
    }
	
}
