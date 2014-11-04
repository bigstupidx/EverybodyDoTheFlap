using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;

public class UtilEditor : EditorWindow
{
    public GUIStyle debuglineStyle;
    [SerializeField]
    public int drawType = 1;
    [SerializeField]
    public Color defaultColor = Color.green;
    [SerializeField]
    public bool textDebugging = true;
    [SerializeField]
    public bool objectDebugging = true;
    [SerializeField]
    public bool logTags = true;
    [SerializeField]
    public int debugLineTextSize = 12;
    [SerializeField]
    public int debugObjectTextSize = 16;
    [SerializeField]
    public Color debugLineColor = Color.white;
    [SerializeField]
    public Color debugObjectColor = Color.white;
    [SerializeField]
    public bool debugLineBackground = true;
    [SerializeField]
    public bool debugObjectBackground = true;
    [SerializeField]
    public bool scaleWithDistance = true;
    [SerializeField]
    private Vector2 scrollPos;
    [SerializeField]
    public float debugWindowWidth = 0.4f;
    [SerializeField]
    public float debugWindowHeight = 0.3f;
    [SerializeField]
    public bool drawGraph = true;
    [SerializeField]
    public Rect graphRect = new Rect(10, 10, 350, 100);
    [SerializeField]
    public float graphUpdateTime = 0.05f;


    string[] LogTagNames = System.Enum.GetNames(typeof(UtilLogTag));
    Color[] LogTagColors = UtilLogTagColors._Color;

    [MenuItem("Window/Unitility/Util Editor")]
    static void Init()
    {
        // Get existing open window if one exist, otherwise create a new one
        UtilEditor window = (UtilEditor)EditorWindow.GetWindow(typeof(UtilEditor));
        window.Load();
    }

    static UtilOnGUI _UtilGUI;

    void Update()
    {
        if (Time.timeSinceLevelLoad > 0 && Time.timeSinceLevelLoad < 1)
        {
            UpdateValues();
        }
    }

    string[] names = { "Don't draw", "GL_Lines", "Gizmo Lines" };
    int[] values = { 0, 1, 2 };


    void OnGUI()
    {
        bool update = false;

        Color c;
        bool b;
        int i;
        float f;

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUIStyle.none);
        //visualDebugging = EditorGUILayout.BeginToggleGroup("Text Debugging", visualDebugging);
        EditorGUILayout.LabelField(new GUIContent("General options"), EditorStyles.boldLabel);
        //EditorGUILayout.HelpBox("General options for debugging and visual appearance", MessageType.Info);

        //TODO: FIX ME! I'm always 0 for some reason
        i = EditorGUILayout.IntPopup("Draw shapes as:", drawType, names, values);
        if (drawType != i) { Undo.RecordObject(this, "Set DrawType"); drawType = i; update = true; EditorUtility.SetDirty(this); }

        c = EditorGUILayout.ColorField(new GUIContent("Default shape color", "The color used by default in most Util visual debugging methods.\n(for instance: DrawSphere() will draw with this color if you do not specify another color)"), defaultColor);
        if (defaultColor != c)
        {
            Undo.RecordObject(this, "Set DrawColor");
            defaultColor = c; update = true;
            EditorUtility.SetDirty(this);
        }


        EditorGUILayout.Separator();


        b = EditorGUILayout.BeginToggleGroup(new GUIContent("Text Debugging", "Uncheck TextDebugging to disable text messages sent via DebugLine() and DebugLinePersistent()"), textDebugging);
        if (textDebugging != b) { Undo.RecordObject(this, "Set DrawText"); textDebugging = b; update = true; EditorUtility.SetDirty(this); }
        EditorGUILayout.BeginHorizontal(GUIStyle.none);
        GUILayout.Label("Width:");
        f = EditorGUILayout.Slider(debugWindowWidth, 0.2f, 0.99f);
        if (debugWindowWidth != f) { Undo.RecordObject(this, "Set TextWidth"); debugWindowWidth = f; update = true; EditorUtility.SetDirty(this); }
        GUILayout.Label("Height:");
        f = EditorGUILayout.Slider(debugWindowHeight, 0.05f, 0.99f);
        if (debugWindowHeight != f) { Undo.RecordObject(this, "Set TextHeight"); debugWindowHeight = f; update = true; EditorUtility.SetDirty(this); }
        EditorGUILayout.EndHorizontal();
        i = (int)EditorGUILayout.IntSlider(new GUIContent("Debug line text size", "The size of the text used in DebugLine() and DebugLinePersistent()"), debugLineTextSize, 8, 20);
        if (debugLineTextSize != i) { Undo.RecordObject(this, "Set TextSize"); debugLineTextSize = i; update = true; EditorUtility.SetDirty(this); }
        c = EditorGUILayout.ColorField(new GUIContent("Debug line text color", "The color of the text used in DebugLine() and DebugLinePersistent()"), debugLineColor);
        if (debugLineColor != c) { Undo.RecordObject(this, "Set TextColor"); debugLineColor = c; update = true; EditorUtility.SetDirty(this); }
        b = EditorGUILayout.Toggle(new GUIContent("Debug line background", "Whether or not the persistent debug lines should have a background"), debugLineBackground);
        if (debugLineBackground != b) { Undo.RecordObject(this, "Set TextBackground"); debugLineBackground = b; update = true; EditorUtility.SetDirty(this); }

        EditorGUILayout.EndToggleGroup();


        EditorGUILayout.Separator();


        b = EditorGUILayout.BeginToggleGroup(new GUIContent("Game Object Debugging", "Uncheck Game Object Debugging to disable debug messages sent via DebugOnObject()"), objectDebugging);
        if (objectDebugging != b) { Undo.RecordObject(this, "Set ObjectDebugging"); objectDebugging = b; update = true; EditorUtility.SetDirty(this); }
        i = (int)EditorGUILayout.IntSlider(new GUIContent("Debug Object text size", "The size of the text used in DebugOnObject()"), debugObjectTextSize, 10, 100);
        if (debugObjectTextSize != i) { Undo.RecordObject(this, "Set ObjectDebuggingSize"); debugObjectTextSize = i; update = true; EditorUtility.SetDirty(this); }
        c = EditorGUILayout.ColorField(new GUIContent("Debug line text color", "The color of the text used in DebugLine() and DebugLinePersistent()"), debugObjectColor);
        if (debugObjectColor != c) { Undo.RecordObject(this, "Set ObjectDebugging color"); debugObjectColor = c; update = true; EditorUtility.SetDirty(this); }
        b = EditorGUILayout.Toggle(new GUIContent("Debug object background", "Whether or not the persistent debug lines should have a background"), debugObjectBackground);
        if (debugObjectBackground != b) { Undo.RecordObject(this, "Set ObjectDebugging background"); debugObjectBackground = b; update = true; EditorUtility.SetDirty(this); }
        b = EditorGUILayout.Toggle(new GUIContent("Scale with distance", "Uncheck to disable scaling of windows based on distance to Game Object"), scaleWithDistance);
        if (scaleWithDistance != b) { Undo.RecordObject(this, "Set ObjectDebug scaling"); scaleWithDistance = b; update = true; EditorUtility.SetDirty(this); }
        EditorGUILayout.EndToggleGroup();


        EditorGUILayout.Separator();


        b = EditorGUILayout.BeginToggleGroup(new GUIContent("Graph Values", "Uncheck Graph Values to disable displaying the information though Graph()"), drawGraph);
        if (drawGraph != b) { Undo.RecordObject(this, "Set GraphDrawing"); drawGraph = b; update = true; EditorUtility.SetDirty(this); }
        Rect r = (Rect)EditorGUILayout.RectField(graphRect);
        if (graphRect != r) { Undo.RecordObject(this, "Set GraphRect"); graphRect = r; update = true; EditorUtility.SetDirty(this); }
        f = EditorGUILayout.FloatField(new GUIContent("Graph Update Time", "The time between the graph debugger updates the values. 0 means every frame"), graphUpdateTime);
        if (graphUpdateTime != f) { Undo.RecordObject(this, "Set GraphUpdateTime"); graphUpdateTime = f; update = true; EditorUtility.SetDirty(this); }

        EditorGUILayout.EndToggleGroup();


        EditorGUILayout.Separator();


        logTags = EditorGUILayout.Foldout(logTags, new GUIContent("Log Tags", "Log Tags are used in Util.Debug.Log functions to categorize logs and to enable sorting. Default tag cannot be altered."));
        if (logTags)
        {
            int deletor = -1;
            for (int ind = 0; ind < LogTagNames.Length; ind++)
            {
                if (EditorApplication.isCompiling)
                {
                    EditorGUILayout.LabelField("Updating!...");
                    Repaint();
                    break;
                }
                EditorGUILayout.BeginHorizontal();

                int _halfwidth = 55;

                if (LogTagNames[ind] != "Default" && LogTagNames[ind] != "Unity")
                {
                    LogTagNames[ind] = EditorGUILayout.TextField(LogTagNames[ind]);
                    LogTagColors[ind] = EditorGUILayout.ColorField(LogTagColors[ind], GUILayout.Width(_halfwidth));
                    if (GUILayout.Button(new GUIContent("Delete", "Click to delete the Log Tag \"" + LogTagNames[ind] + "\""), GUILayout.Width(_halfwidth)))
                    {
                        deletor = ind;
                    }
                }
                else
                {
                    EditorGUILayout.LabelField(new GUIContent(LogTagNames[ind], "The \"" + LogTagNames[ind] + "\" Log Tag is white and cannot be altered"));
                }

                EditorGUILayout.EndHorizontal();
            }

            if (deletor != -1)
                DeleteLogTag(deletor);

            if (!EditorApplication.isCompiling)
            {
                if (GUILayout.Button("Add New Tag", GUILayout.Width(100)))
                {
                    AddLogTag("NewLogTag");

                    Util.Debug.Log("Hello", UtilLogTag.Other);
                }


                Color org = GUI.color;

                if (HasColorChanged() || !LogTagNames.SequenceEqual(System.Enum.GetNames(typeof(UtilLogTag))))
                {
                    GUI.color = Color.yellow;
                }
                if (Application.isPlaying)
                {
                    GUI.color = Color.red;
                }
                if (GUILayout.Button("Saves Tags", GUILayout.Width(100)) && !Application.isPlaying)
                {
                    SaveLogTags();
                }
                GUI.color = org;
            }

            EditorGUILayout.Space();
        }

        EditorGUILayout.EndScrollView();


        if (update)
            UpdateValues();
    }

    bool HasColorChanged()
    {
        if (LogTagColors.Length == UtilLogTagColors._Color.Length)
        {
            for (int i = 0; i < LogTagColors.Length; i++)
            {
                if (LogTagColors[i] != UtilLogTagColors._Color[i])
                    return true;
            }
            return false;
        }
        else
            return true;
    }

    void DeleteLogTag(int tagIndex)
    {
        List<string> temp = new List<string>();
        foreach (string s in LogTagNames)
        {
            temp.Add(s);
        }
        temp.RemoveAt(tagIndex);
        LogTagNames = temp.ToArray();

        List<Color> temp2 = new List<Color>();
        foreach (Color c in LogTagColors)
        {
            temp2.Add(c);
        }
        temp2.RemoveAt(tagIndex);
        LogTagColors = temp2.ToArray();
    }

    void AddLogTag(string tagName)
    {
        List<string> temp = new List<string>();
        foreach (string s in LogTagNames)
        {
            temp.Add(s);
        }
        temp.Add(tagName);
        LogTagNames = temp.ToArray();

        List<Color> temp2 = new List<Color>();
        foreach (Color c in LogTagColors)
        {
            temp2.Add(c);
        }
        temp2.Add(Color.white);
        LogTagColors = temp2.ToArray();
    }

    string GetColorString(Color c)
    {
        return "new Color(" + c.r + "f," + c.g + "f," + c.b + "f," + c.a + "f)";
    }

    void SaveLogTags()
    {
        if (!ContainsDuplicateNames() && ContainsOnlyLetters())
        {
            string s_colors = "";
            foreach (Color c in LogTagColors)
            {
                s_colors += GetColorString(c) + ",";
            }
            s_colors = s_colors.Substring(0, s_colors.Length - 1);

            string s_names = "";
            for (int i = 0; i < LogTagNames.Length; i++)
            {
                s_names += LogTagNames[i].ToString() + " = " + Mathf.Pow(2, i) + ",";
            }
            s_names = s_names.Substring(0, s_names.Length - 1);

            string s =
                "using System;\n" +
                "using System.Collections.Generic;\n" +
                "using UnityEngine;\n" +
                "//This class is off limits!!!\n" +
                "public class UtilLogTagColors\n" +
                "{\n" +
                "	private static Color[] _color = {" + s_colors + "};\n" +
                "	public static Color[] _Color \n" +
                "	{\n" +
                "		get{ return _color; }\n" +
                "	}\n" +
                "}\n" +

                "public enum UtilLogTag {" + s_names + "}";


            File.WriteAllText(Application.dataPath + "/Standard Assets/Util/UtilLogTag.cs", s);
            AssetDatabase.Refresh();
        }
        else
        {
            Debug.LogError("UtilEditor:SaveLogTags()\nCould not save LogTags. Please check that all LogTagNames are unique.");
        }
    }

    bool ContainsDuplicateNames()
    {
        List<string> names = new List<string>();
        foreach (string name in LogTagNames)
        {
            if (names.Contains(name))
                return true;
            else
                names.Add(name);
        }
        return false;
    }

    bool ContainsOnlyLetters()
    {
        foreach (string name in LogTagNames)
        {
            Match match = Regex.Match(name, @"[^a-z][^a-z_]", RegexOptions.IgnoreCase);
            if (match.Success)
                return false;
        }
        return true;
    }

    void UpdateValues()
    {
        Util.Draw._shapeColor = defaultColor;
        Util.Settings._shapeDrawType = drawType;


        if (Util.Settings.enabled)
        {
            Util.OnGUI = textDebugging;
            Util._UtilOnGUI.GetComponent<UtilOnGUI>().textSize = debugLineTextSize;
            Util._UtilOnGUI.GetComponent<UtilOnGUI>().textColor = debugLineColor;
            Util._UtilOnGUI.GetComponent<UtilOnGUI>().textBackground = debugLineBackground;
            Util._UtilOnGUI.GetComponent<UtilOnGUI>().debugWindowWidth = debugWindowWidth;
            Util._UtilOnGUI.GetComponent<UtilOnGUI>().debugWindowHeight = debugWindowHeight;

            Util.OnGUIObject = objectDebugging;
            Util._UtilOnGUI.GetComponent<UtilOnGUI>().objectTextSize = debugObjectTextSize;
            Util._UtilOnGUI.GetComponent<UtilOnGUI>().objectTextColor = debugObjectColor;
            Util._UtilOnGUI.GetComponent<UtilOnGUI>().objectTextBackground = debugObjectBackground;
            Util._UtilOnGUI.GetComponent<UtilOnGUI>().scaleWithDistace = scaleWithDistance;

            UtilGraph.DrawGraphs = drawGraph;
            UtilGraph.ChangeOutlineSize(graphRect);
            UtilGraph.updateTime = graphUpdateTime;
        }
    }

    void OnEnable()
    {
        Load();
        UpdateValues();
    }

    void OnDisable()
    {
        Save();
    }

    void OnLostFocus()
    {
        Save();
    }

    //Possibly: OnDestroy, OnFocus

    void Save()
    {
        //Debug.Log("Saving...");
        //Booleans
        EditorPrefs.SetBool("textDebugging", textDebugging);
        EditorPrefs.SetBool("objectDebugging", objectDebugging);
        EditorPrefs.SetBool("debugLineBackground", debugLineBackground);
        EditorPrefs.SetBool("debugObjectBackground", debugObjectBackground);
        EditorPrefs.SetBool("scaleWithDistance", scaleWithDistance);
        EditorPrefs.SetBool("drawGraph", drawGraph);

        //Integers
        EditorPrefs.SetInt("drawShapeType", drawType);
        EditorPrefs.SetInt("debugLineTextSize", debugLineTextSize);
        EditorPrefs.SetInt("debugObjectTextSize", debugObjectTextSize);

        //Floats
        EditorPrefs.SetFloat("debugWindowWidth", debugWindowWidth);
        EditorPrefs.SetFloat("debugWindowHeight", debugWindowHeight);
        EditorPrefs.SetFloat("graphUpdateTime", graphUpdateTime);

        //Colors
        EditorPrefs.SetFloat("shapeColorR", defaultColor.r);
        EditorPrefs.SetFloat("shapeColorG", defaultColor.g);
        EditorPrefs.SetFloat("shapeColorB", defaultColor.b);

        EditorPrefs.SetFloat("debugLineColorR", debugLineColor.r);
        EditorPrefs.SetFloat("debugLineColorG", debugLineColor.g);
        EditorPrefs.SetFloat("debugLineColorB", debugLineColor.b);

        EditorPrefs.SetFloat("debugObjectColorR", debugObjectColor.r);
        EditorPrefs.SetFloat("debugObjectColorG", debugObjectColor.g);
        EditorPrefs.SetFloat("debugObjectColorB", debugObjectColor.b);

        //Rects
        EditorPrefs.SetInt("graphRectX", (int)graphRect.x);
        EditorPrefs.SetInt("graphRectY", (int)graphRect.y);
        EditorPrefs.SetInt("graphRectW", (int)graphRect.width);
        EditorPrefs.SetInt("graphRectH", (int)graphRect.height);
        //Debug.Log("Saved!");
    }

    void Load()
    {
        //Debug.Log("Loading...");
        //Booleans
        if (EditorPrefs.HasKey("textDebugging"))
            textDebugging = EditorPrefs.GetBool("textDebugging");

        if (EditorPrefs.HasKey("objectDebugging"))
            objectDebugging = EditorPrefs.GetBool("objectDebugging");

        if (EditorPrefs.HasKey("debugLineBackground"))
            debugLineBackground = EditorPrefs.GetBool("debugLineBackground");

        if (EditorPrefs.HasKey("debugObjectBackground"))
            debugObjectBackground = EditorPrefs.GetBool("debugObjectBackground");

        if (EditorPrefs.HasKey("scaleWithDistance"))
            scaleWithDistance = EditorPrefs.GetBool("scaleWithDistance");

        if (EditorPrefs.HasKey("drawGraph"))
            drawGraph = EditorPrefs.GetBool("drawGraph");

        //Integers
        if (EditorPrefs.HasKey("drawShapeType"))
            drawType = EditorPrefs.GetInt("drawShapeType");

        if (EditorPrefs.HasKey("debugLineTextSize"))
            debugLineTextSize = EditorPrefs.GetInt("debugLineTextSize");

        if (EditorPrefs.HasKey("debugObjectTextSize"))
            debugObjectTextSize = EditorPrefs.GetInt("debugObjectTextSize");

        //Floats
        if (EditorPrefs.HasKey("debugWindowWidth"))
            debugWindowWidth = EditorPrefs.GetFloat("debugWindowWidth");

        if (EditorPrefs.HasKey("debugWindowHeight"))
            debugWindowHeight = EditorPrefs.GetFloat("debugWindowHeight");

        if (EditorPrefs.HasKey("graphUpdateTime"))
            graphUpdateTime = EditorPrefs.GetFloat("graphUpdateTime");

        //Colors
        if (EditorPrefs.HasKey("shapeColorR"))
            defaultColor = new Color(EditorPrefs.GetFloat("shapeColorR"), EditorPrefs.GetFloat("shapeColorG"), EditorPrefs.GetFloat("shapeColorB"), 1);

        if (EditorPrefs.HasKey("debugLineColorR"))
            debugLineColor = new Color(EditorPrefs.GetFloat("debugLineColorR"), EditorPrefs.GetFloat("debugLineColorG"), EditorPrefs.GetFloat("debugLineColorB"), 1);

        if (EditorPrefs.HasKey("debugObjectColorR"))
            debugObjectColor = new Color(EditorPrefs.GetFloat("debugObjectColorR"), EditorPrefs.GetFloat("debugObjectColorG"), EditorPrefs.GetFloat("debugObjectColorB"), 1);

        //Rects
        if (EditorPrefs.HasKey("graphRectX"))
            graphRect = new Rect(EditorPrefs.GetInt("graphRectX"), EditorPrefs.GetInt("graphRectY"), EditorPrefs.GetInt("graphRectW"), EditorPrefs.GetInt("graphRectH"));
        //Debug.Log("Loaded!");
    }

}

[InitializeOnLoad]
class UtilHierarchyIcon
{
    static Texture2D texture;
    static List<int> markedObjects;

    static UtilHierarchyIcon()
    {
        // Init
        texture = AssetDatabase.LoadAssetAtPath("Assets/Standard Assets/Util/Resources/UtilLogo.png", typeof(Texture2D)) as Texture2D;
        EditorApplication.update += UpdateCB;
        EditorApplication.hierarchyWindowItemOnGUI += HierarchyItemCB;
    }

    static void UpdateCB()
    {
        // Check here
        //GameObject[] go = Object.FindObjectsOfType(typeof(GameObject)) as GameObject[];
        UtilOnGUI[] go = Object.FindObjectsOfType(typeof(UtilOnGUI)) as UtilOnGUI[];
        markedObjects = new List<int>();
        foreach (UtilOnGUI g in go)
        {
            // Example: mark all lights
            //if (g.GetComponent<UtilOnGUI>() != null)
            markedObjects.Add(g.gameObject.GetInstanceID());
        }

    }

    static void HierarchyItemCB(int instanceID, Rect selectionRect)
    {
        // place the icoon to the right of the list:
        Rect r = new Rect(selectionRect);
        r.x = r.width - 5;
        r.width = 100;
        r.height = 32;

        if (markedObjects != null && markedObjects.Contains(instanceID))
        {
            // Draw the texture if it's a light (e.g.)
            GUI.Label(r, texture);
        }
    }

}