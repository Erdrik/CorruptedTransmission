using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandsManager : MonoBehaviour {

    public Animator _animator;

    public void OpenMoveMenu()
    {
        _animator.SetBool("MoveMenu", true);
    }

    public void CloseMoveMenu()
    {
        _animator.SetBool("MoveMenu", false);
    }
	

}
