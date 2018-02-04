using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinRoom : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        Professor p = other.GetComponent<Professor>();
        if (p)
        {
            RoomCameraManager.EndGame();
        }
    }

}
