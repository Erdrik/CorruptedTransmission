using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {
	// Update is called once per frame
	void Update () {
        int seconds = Mathf.FloorToInt((Time.time % 60));
        int minutes = Mathf.FloorToInt((Time.time / 60));
        GetComponent<Text>().text = ((minutes < 10) ? "0" : "") + minutes + ":" + ((seconds < 10) ? "0" : "")  + seconds;

	}
}
