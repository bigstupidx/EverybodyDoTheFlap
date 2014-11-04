using UnityEngine;
using System.Collections;

public class IntenseLight : MonoBehaviour {

    public float intensity = 10;
    Light light;

	// Use this for initialization
	void Start () {
        light = GetComponent<Light>();
        
	}
	
	// Update is called once per frame
	void Update () {
        light.intensity = intensity;
	}
}
