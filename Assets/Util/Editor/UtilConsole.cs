using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class UtilConsole : EditorWindow
{
    List<Util.Debug._DebugLine> _debugLines;
    static UtilConsole window;

    [MenuItem("Window/Unitility/Util Console")]
    static void Init()
    {
        // Get existing open window if one exist, otherwise create a new one
        window = (UtilConsole)EditorWindow.GetWindow(typeof(UtilConsole));
    }

    int logTags = -1;
    bool Collapse;
    bool ClearOnPlay;
    Vector2 scrollPos;
    int selected = -1;
    int height = 50;
    bool dragging;

    static void AddLog(Util.Debug._DebugLine line)
    {
        if (window._debugLines == null)
            window._debugLines = new List<Util.Debug._DebugLine>();
        window._debugLines.Add(line);
    }

    // Update is called once per frame
    void OnGUI()
    {
        DrawConsoleLog(Util.Debug.EditorConsoleLog.editorLog);
    }

    private void DrawConsoleLog(List<Util.Debug._DebugLine> debugLines)
    {
        ToolbarGUI();
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        if (debugLines != null)
        {
            for (int i = 0; i < debugLines.Count; i++)
            {

                if ((logTags ^ (int)debugLines[i].tag) < logTags)
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
                    EditorGUILayout.LabelField("", GUILayout.Width(25));
                    EditorGUILayout.LabelField(debugLines[i].line + "\n" + debugLines[i].tag, EditorStyles.wordWrappedLabel);
                    if (GUI.Button(r, "", EditorStyles.label))//"Open", GUILayout.Width(45)))
                    {
                        if (selected == i)
                            OpenStackTrace(debugLines[i].stackTrace.GetFrame(debugLines[i].stackTrace.FrameCount - 1));
                        selected = i;
                    }

                    t.SetPixel(0, 0, UtilLogTagColors._Color[Mathf.Max(0, (int)Mathf.Log((int)debugLines[i].tag, 2))]);
                    //Debug.Log(Mathf.Max(0, (int)Mathf.Log(2, (int)debugLines[i].tag)));
                    //t.SetPixel(0, 0, UtilLogTagColors._Color[Mathf.Max(0, (int)Mathf.Log(2, (int)debugLines[i].tag))]);

                    t.Apply();
                    GUI.DrawTexture(new Rect(r.x + 5, r.y + 5, 20, r.height - 10), t);

                    EditorGUILayout.EndHorizontal();
                }
            }
        }
        else
            selected = -1;
        EditorGUILayout.EndScrollView();
        Rect h = EditorGUILayout.BeginHorizontal();
        EditorGUIUtility.AddCursorRect(h, MouseCursor.ResizeVertical);

        GUILayout.Box("", GUILayout.Height(7), GUILayout.Width(Screen.width - 10));

        EditorGUILayout.EndHorizontal();

        scrollPos2 = EditorGUILayout.BeginScrollView(scrollPos2, GUILayout.Height(height));
        if (Util.Debug.DebugLines != null && Util.Debug.DebugLines.Count > selected && selected != -1)
            ListStackTrace(selected, height);
        else
            selected = -1;
        EditorGUILayout.EndScrollView();

        Resize(h);
    }

    Vector2 scrollPos2 = new Vector2();

    private void ToolbarGUI()
    {
        EditorGUILayout.BeginHorizontal();
        logTags = (int)((UtilLogTag)EditorGUILayout.EnumMaskField("", (UtilLogTag)logTags, GUILayout.Width(100)));

        GUILayout.Label("", EditorStyles.toolbarButton, GUILayout.Width(5));
        if (GUILayout.Button("Clear", EditorStyles.toolbarButton, GUILayout.Width(43)))
        {
            Util.Debug.ClearLog();
        }
        GUILayout.Label("", EditorStyles.toolbarButton, GUILayout.Width(7));
        Collapse = GUILayout.Toggle(Collapse, "Collapse", EditorStyles.toolbarButton, GUILayout.Width(55));
        ClearOnPlay = GUILayout.Toggle(ClearOnPlay, "Clear on Play", EditorStyles.toolbarButton, GUILayout.Width(85));
        EditorGUILayout.EndHorizontal();
    }

    void ListStackTrace(int selected, int height)
    {
        for (int i = Util.Debug.DebugLines[selected].stackTrace.FrameCount - 1; i >= 0; i--)
        //foreach (System.Diagnostics.StackFrame frame in Util.Debug.DebugLines[selected].stackTrace.GetFrames())
        {
            System.Diagnostics.StackFrame frame = Util.Debug.DebugLines[selected].stackTrace.GetFrames()[i];
            if (frame.GetFileName() == null || frame.GetFileName() == "")
                continue;
            string[] names = frame.GetFileName().Split('\\');
            Rect r = EditorGUILayout.BeginHorizontal();
            //
            string s = names[names.Length - 1] + ":" + frame.GetMethod().Name + " - " + frame.GetFileLineNumber();
            EditorGUILayout.SelectableLabel(s, GUILayout.Height(16));
            if (GUILayout.Button(" ", GUILayout.Width(16)))
                OpenStackTrace(frame);
            EditorGUILayout.EndHorizontal();


            /*if (r.Contains(Event.current.mousePosition))
            {
                if (GUI.Button(new Rect(r.x + r.width - 50, r.y + r.height - 16, 50, 16), "Open"))
                    OpenStackTrace(frame);

            }
            else
            {
                EditorGUI.SelectableLabel(r, s);
            }*/


            //EditorGUILayout.SelectableLabel(names[names.Length - 1] + ":" + frame.GetMethod().Name + " - " + frame.GetFileLineNumber(), GUILayout.Height(16));
        }

    }

    void OpenStackTrace(System.Diagnostics.StackFrame frame)
    {
        UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(@"" + frame.GetFileName(), frame.GetFileLineNumber());
    }

    void OpenScript(string stackTrace)
    {
        int line = 1;
        string fileLocation = Application.dataPath;
        string[] split1 = stackTrace.Split(':');
        string[] split2 = split1[split1.Length - 2].Split('(', ')');
        string str = "";
        foreach (string s in split2)
        {
            if (s.Contains("Assets"))
            {
                split2 = s.Split(':');
                line = Int32.Parse(split1[split1.Length - 1].Split(')')[0]);
                str = split2[0].Replace("\\", "/");
                str = str.Substring(9, split2[0].Length - 9);
                break;
            }
        }

        UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(@"" + fileLocation + str, line);
    }

    void Resize(Rect r)
    {
        if (Event.current != null)
        {
            switch (Event.current.type)
            {
                case EventType.MouseDown:
                    if (r.Contains(Event.current.mousePosition))
                    {
                        dragging = true;
                    }
                    break;
                case EventType.MouseDrag:
                    if (dragging)
                    {
                        height -= (int)Event.current.delta.y;
                        if (height < 20)
                            height = 20;
                        Repaint();
                    }
                    break;
                case EventType.MouseUp:
                    if (dragging)
                    {
                        dragging = false;
                    }
                    break;
            }
        }
    }

    void Update()
    {
        Repaint();
    }
}
