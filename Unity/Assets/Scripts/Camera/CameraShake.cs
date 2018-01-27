using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

    [Range(0,5)]
    public float _intensity;
    public float _speed = 2;
    public float _shakeSpeed = 10;

    public bool _constantShake;
    public Transform _cameraTransform;

    private float _actualIntensity;
    private Vector3 _orignalPosition;

    private void Start()
    {
        _orignalPosition = _cameraTransform.localPosition;
    }

    // Update is called once per frame
    void Update () {
        if (_constantShake)
        {
            _actualIntensity = _intensity;
        }
        else
        {
            if (_actualIntensity > 0)
            {
                _actualIntensity -= _speed * Time.deltaTime;
            }
            else
            {
                _actualIntensity = 0;
            }
        }
        ShakeCamera();

    }

    [ContextMenu("Test Shake")]
    public void DoShake()
    {
        DoShake(_intensity);
    }

    public void DoShake(float intensity)
    {
        _actualIntensity = intensity;
    }

    void ShakeCamera()
    {
        float exponentialIntensity = _actualIntensity * _actualIntensity * _actualIntensity;
        _cameraTransform.localPosition = _orignalPosition + transform.right * ((Mathf.PerlinNoise(Time.time * _shakeSpeed, 0)*2)-1) * exponentialIntensity + transform.up * ((Mathf.PerlinNoise(0,Time.time * _shakeSpeed) * 2) - 1) * exponentialIntensity;
    }
}
