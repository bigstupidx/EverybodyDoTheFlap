using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class SnapToGrid : MonoBehaviour {

    void Awake()
    {
#if UNITY_EDITOR
        if (UnityEditor.EditorApplication.isPlaying)
            Destroy(this);
#else
        Destroy(this);
#endif

    }
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(Mathf.RoundToInt(transform.position.x), 0.5f, Mathf.RoundToInt(transform.position.z));
	}
}
