using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour {

    static CameraShake instance;

	// Use this for initialization
	void Start () {
        instance = this;
	}

    public static void Shake(float magnitude, float duration)
    {
        instance.StartCoroutine(instance.ShakeRoutine(magnitude, duration));
    }

    IEnumerator ShakeRoutine(float magnitude, float duration)
    {

        float elapsed = 0.0f;

        Vector3 originalCamPos = transform.position;

        while (elapsed < duration)
        {

            elapsed += Time.deltaTime;

            float percentComplete = elapsed / duration;
            float damper = 1 - percentComplete;//1.0f - Mathf.Clamp(4.0f * percentComplete - 3.0f, 0.0f, 1.0f);

            transform.position = originalCamPos + Util.Random.Vector3(magnitude * damper, magnitude * damper);

            yield return null;
        }

        transform.position = originalCamPos;
    }
}
