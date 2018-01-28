using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour {

    public AudioMixer _mixer;
    public AudioSource _music;

    [Range(0.5f,2)]
    public float _pitchModifer = 1f;
    [Range(10, 22000)]
    public float _cutoffModifer = 5000;

    public float _monsterPitchDistance = 40f;

    public void Update()
    {
        SetCutoff(_cutoffModifer);
        CheckMonsterRadius();
    }

    private void CheckMonsterRadius()
    {
        if (RoomCameraManager._professor)
        {
            float dist = Vector3.Distance(RoomCameraManager._professor.transform.position, RoomCameraManager._professor._nemesis.transform.position);
            if(dist < _monsterPitchDistance)
            {
                SetPitch((dist / (_monsterPitchDistance * 2)) + 0.5f);
            }
            else
            {
                SetPitch(1);
            }
        }
    }

    public void SetPitch(float newPitch)
    {
        _mixer.SetFloat("pitch", newPitch);
    }

    public void SetCutoff(float newCutoff)
    {
        _mixer.SetFloat("cutoff", newCutoff);
    }

}
