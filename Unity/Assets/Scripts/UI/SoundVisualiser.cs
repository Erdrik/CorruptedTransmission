using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundVisualiser : MonoBehaviour {

    public LineRenderer _lineRenderer;
    public float _intensity = 50;
    public float _scale = 1;
    public int _detail = 64;

    private void Start()
    {
        _lineRenderer.positionCount = _detail;
    }

    // Update is called once per frame
    void Update () {
        float[] output = new float[_detail];
        AudioListener.GetOutputData(output, 0);
        Vector3[] normalisedOutput = new Vector3[_detail];
        for (int i = 0; i < output.Length; i++)
        {
            normalisedOutput[i] = new Vector3((i-(_detail * 0.5f))*_scale/_detail,output[i] * _intensity,0);
        }
        _lineRenderer.SetPositions(normalisedOutput);
	}
}
