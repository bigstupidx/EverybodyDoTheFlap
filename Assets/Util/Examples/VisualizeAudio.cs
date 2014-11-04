using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class VisualizeAudio : MonoBehaviour 
{
    public bool GraphValue = false;
    public bool DisplaySphere = true;

    public int sampleSize = 2048;
    float[] samples;
    float visualizedAmplitude = 0;
    float amplitude = 0;
	void Start()
    {
        samples = new float[sampleSize];
    }
	// Update is called once per frame
	void Update () 
    {
        if (sampleSize != samples.Length)
            samples = new float[sampleSize];

        GetComponent<AudioSource>().GetOutputData(samples, 0);

        visualizedAmplitude = Mathf.Lerp(visualizedAmplitude, 0, Util.deltaTime * 5);

        amplitude = Util.Math.AverageAbs(samples);
        if (amplitude > visualizedAmplitude)
            visualizedAmplitude = amplitude;


        if (DisplaySphere)
            Util.Draw.Sphere(transform.position, visualizedAmplitude, Color.Lerp(Color.green, Color.red, visualizedAmplitude));
        if (GraphValue)
            Util.Debug.Graph(GetComponent<AudioSource>().clip.name, amplitude);
	}
}
