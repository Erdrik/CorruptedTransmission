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

    private float _corruption = 0;
    private int _corruptSources = 0;

    public void Update()
    {
        SetCutoff(_cutoffModifer);
        AddToCourruption(CheckMonsterRadius());
        AddToCourruption(GuageEmotions());
        PitchCorruption();
    }

    private void PitchCorruption()
    {
        _corruption = Mathf.Clamp01(_corruption / _corruptSources);
        SetPitch(1f - (0.5f * _corruption));
        _corruption = 0;
        _corruptSources = 0;
    }

    private void AddToCourruption(float c)
    {
        _corruption += c;
        _corruptSources++;
    }

    private float GuageEmotions()
    {
        if (RoomCameraManager._professor)
        {
            float emotionalProblemScore = 0;
            emotionalProblemScore += RoomCameraManager._professor._anger;
            emotionalProblemScore += RoomCameraManager._professor._distrust;
            emotionalProblemScore += RoomCameraManager._professor._fear;
            return emotionalProblemScore / 3;
        }
        return 0;
    }

    private float CheckMonsterRadius()
    {
        if (RoomCameraManager._professor && RoomCameraManager._nemisis)
        {
            float dist = Vector3.Distance(RoomCameraManager._professor.transform.position, RoomCameraManager._nemisis.transform.position);
            if(dist < _monsterPitchDistance)
            {
                return  1-(dist / _monsterPitchDistance);
            }
        }
        return 0;
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
