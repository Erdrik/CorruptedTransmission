using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraFeed : MonoBehaviour {

    private RenderTexture _currentTexture;
    private bool isActive = true;

    public void Start()
    {
        GetComponentInChildren<RawImage>().material = new Material(GetComponentInChildren<RawImage>().material);
    }

    public void SetTexture(RenderTexture texture)
    {
        _currentTexture = texture;
    }

    public void DisableTexture()
    {
        if (isActive)
        {
            GetComponentInChildren<RawImage>().material.mainTexture = null;
            isActive = false;
        }
    }

    public void EnableTexture()
    {
        if (!isActive)
        {
            GetComponentInChildren<RawImage>().material.mainTexture = _currentTexture;
            isActive = true;
        }
    }

    public void Init(CameraController c)
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            if (isActive)
            {
                RoomCameraManager.GoToCamera(c);
                GetComponent<Button>().OnDeselect(new UnityEngine.EventSystems.BaseEventData(UnityEngine.EventSystems.EventSystem.current));
            }
        });
    }

}
