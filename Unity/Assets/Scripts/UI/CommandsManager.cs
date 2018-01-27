using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandsManager : MonoBehaviour {

    public Animator _animator;
    public MakeNiceProtocol _protocol;

    public Button _placeButtonPrefab;
    public RectTransform _placeButtonRoot;
    public List<Button> _placeButtons = new List<Button>();

    public void Start()
    {
        BuildPlaceButtons();
    }

    public void OpenMoveMenu()
    {
        _protocol.InstructMove();
        _animator.SetBool("MoveMenu", true);
    }

    public void CloseMoveMenu()
    {
        _animator.SetBool("MoveMenu", false);
    }

    public void MoveTo(Room room)
    {
        _protocol.InstructRoom(room);
    }

    public void BuildPlaceButtons()
    {
        if (RoomCameraManager._instance)
        {
            foreach(Room r in RoomCameraManager._instance._rooms)
            {
                Button button = Instantiate(_placeButtonPrefab);
                button.GetComponent<RectTransform>().SetParent(_placeButtonRoot, false);
                button.GetComponentInChildren<Text>().text = r._tag;
                button.onClick.AddListener(() =>
                {
                    MoveTo(r);
                });
                _placeButtons.Add(button);
            }
        }
    }

    public void Stop()
    {
        _protocol.InstructStop();
    }

    public void GetOut()
    {
        _protocol.InstructGetOut();
    }
	
    public void Hide()
    {
        _protocol.InstructHide();
    }

    public void Activate()
    {
        _protocol.InstructPushButton();
    }

    public void Lock()
    {
        _protocol.InstructLock();
    }

    public void GoBack()
    {
        _protocol.InstructGoBack();
    }

}
