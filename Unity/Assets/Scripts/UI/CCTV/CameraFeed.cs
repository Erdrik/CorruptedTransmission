using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraFeed : MonoBehaviour {

    private RenderTexture _currentTexture;
    private bool isActive = false;

    public Text text;

    public void Start()
    {
        
    }

    public void SetTexture(RenderTexture texture)
    {
        GetComponentInChildren<RawImage>().material = new Material(GetComponentInChildren<RawImage>().material);
        _currentTexture = texture;
    }

    public void DisableTexture()
    {
        if (isActive)
        {
            GetComponentInChildren<RawImage>().material.mainTexture = Texture2D.whiteTexture;
            GetComponentInChildren<RawImage>().color = Color.black;
            text.gameObject.SetActive(true);
            isActive = false;
        }
    }

    public void EnableTexture()
    {
        if (!isActive)
        {
            GetComponentInChildren<RawImage>().material.mainTexture = _currentTexture;
            GetComponentInChildren<RawImage>().color = Color.white;
            text.gameObject.SetActive(false);
            isActive = true;
        }
    }

    public void Init(CameraController c)
    {
        GetComponent<Button>().onClick.RemoveAllListeners();
        GetComponent<Button>().onClick.AddListener(() =>
        {
            if (isActive)
            {
                GameManager.GetCCTVCamera().GoToCamera(c);
                GetComponent<Button>().OnDeselect(new UnityEngine.EventSystems.BaseEventData(UnityEngine.EventSystems.EventSystem.current));
            }
        });
    }

}
