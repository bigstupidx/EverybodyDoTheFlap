using UnityEngine;
using System.Collections;

public class FPSCounter : MonoBehaviour
{
    public bool GraphFPS = false;
    public bool DebugFPS = true;

    public float updateInterval = 0.25F;

    private float accum = 0; // FPS accumulated over the interval
    private int frames = 0; // Frames drawn over the interval
    private float timeleft; // Left time for current interval

    void Start()
    {
        timeleft = updateInterval;
    }

    void Update()
    {
        timeleft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        ++frames;

        // Interval ended - update GUI text and start new interval
        if (timeleft <= 0.0)
        {
            // display two fractional digits (f2 format)
            float fps = accum / frames;
            string format = System.String.Format("{0:F2} FPS", fps);

            if (GraphFPS)
                Util.Debug.Graph("FPS", fps);
            if (DebugFPS)
                Util.Debug.LogPersistent("FPS", format);

            //	DebugConsole.Log(format,level);
            timeleft = updateInterval;
            accum = 0.0F;
            frames = 0;
        }
    }
}
