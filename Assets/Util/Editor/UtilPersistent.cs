using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class UtilPersistent : EditorWindow
{
    [MenuItem("Window/Unitility/Util Persistent Debugs")]
    static void Init()
    {
        // Get existing open window if one exist, otherwise create a new one
        UtilPersistent window = (UtilPersistent)EditorWindow.GetWindow(typeof(UtilPersistent));
    }

    int selected = -1;
    Vector2 scrollPos;

    void OnGUI()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        int i = 0;
        if (UtilOnGUI.StringDict != null)
        {
            //foreach (GameObject goDict in UtilOnGUI.StringDict.Keys)
            foreach (Util.Debug._DebugLine pLine in Util.Debug.DebugLinesPersistent)
            {
                Rect r = EditorGUILayout.BeginHorizontal();
                Texture2D t = new Texture2D(1, 1);
                if (selected == i)
                    t.SetPixel(0, 0, GUI.skin.settings.selectionColor);
                else if (i % 2 == 0)
                    t.SetPixel(0, 0, new Color(0.4f, 0.4f, 0.4f, 0.4f));
                else
                    t.SetPixel(0, 0, new Color(0.0f, 0.0f, 0.0f, 0.0f));
                t.Apply();
                GUI.DrawTexture(r, t);

                EditorGUILayout.LabelField(pLine.line + ": " + pLine.value, EditorStyles.wordWrappedLabel);
                if (GUI.Button(r, "", EditorStyles.label))//"Open", GUILayout.Width(45)))
                {
                    if (selected == i)
                    {

                    }
                    selected = i;
                }
                EditorGUILayout.EndHorizontal();
                i++;
            }
        }
        GUILayout.EndScrollView();
    }

    void Update()
    {
        Repaint();
    }

}
