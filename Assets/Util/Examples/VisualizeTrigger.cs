using UnityEngine;
using System.Collections;

public class VisualizeTrigger : MonoBehaviour
{
    public bool Enter = true;
    public bool Stay = true;
    public bool Exit = true;

    void OnTriggerEnter(Collider col)
    {
        if (Enter)
            Util.Draw.Cube(transform, transform.lossyScale + (Vector3.one * 0.1f), Color.yellow, 0.1f);
    }

    void OnTriggerStay(Collider col)
    {
        if (Stay)
            Util.Draw.Cube(transform, Color.green, 0);
    }

    void OnTriggerExit(Collider col)
    {
        if (Exit)
            Util.Draw.Cube(transform, transform.lossyScale + (Vector3.one * 0.2f), Color.red, 0.1f);
    }
}
