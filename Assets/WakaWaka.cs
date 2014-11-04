using UnityEngine;
using System.Collections;

public class WakaWaka : MonoBehaviour {

    public GameObject top, bottom;
    public float topOpen, topClosed, bottomOpen, bottomClosed;
    public float duration;
    float progress;
    int direction = 1;
	// Update is called once per frame
	void Update () 
    {
        progress += Time.deltaTime * direction;
        if (progress > duration)
            direction = -1;
        else if (progress < 0)
            direction = 1;

        top.transform.localEulerAngles = top.transform.localEulerAngles.X(Mathf.Lerp(topOpen, topClosed, progress / duration));
        bottom.transform.localEulerAngles = bottom.transform.localEulerAngles.X(Mathf.Lerp(bottomOpen, bottomClosed, 1- progress / duration));
	}
}
