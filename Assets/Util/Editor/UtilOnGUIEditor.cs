using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(UtilOnGUI))]
public class UtilOnGUIEditor : Editor {

    private Texture _logo;

    public override void OnInspectorGUI()
    {
        if (_logo == null)
        {
            _logo = (Texture)(Resources.Load("UtilLogoBig"));
        }
        GUILayout.BeginHorizontal();
        GUILayout.Label("", GUILayout.Width((Screen.width / 2) - (_logo.width/2) - 19));
        GUILayout.Label(_logo);
        GUILayout.EndHorizontal();
        
        if (GUILayout.Button("Change Settings", GUILayout.Height(35)))
        {
            EditorWindow.GetWindow(typeof(UtilEditor));
        }
        if (GUILayout.Button("Read Documentation", GUILayout.Height(35)))
        {
            Application.OpenURL("file:///" + Application.dataPath + "/Standard Assets/Util/Script Reference/Script Reference.html");
        }
        if (GUILayout.Button("Go to website", GUILayout.Height(35)))
        {
            Application.OpenURL("www.google.com");
        }
        //base.OnInspectorGUI();
    }
}
