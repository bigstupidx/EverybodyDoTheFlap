using UnityEngine;
using UtilTuple;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Text;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif


[ExecuteInEditMode]
public class UtilOnGUI : MonoBehaviour
{
    [HideInInspector]
    public int textSize = 16;
    [HideInInspector]
    public int objectTextSize = 16;
    [HideInInspector]
    public Color textColor = Color.white;
    [HideInInspector]
    public Color objectTextColor = Color.white;
    [HideInInspector]
    public Color objectBackgroundColor = Color.black;
    [HideInInspector]
    public bool textBackground = true;
    [HideInInspector]
    public bool objectTextBackground = true;
    [HideInInspector]
    public bool onGUI = true;
    [HideInInspector]
    public bool onGUIObjects = true;
    [HideInInspector]
    public bool scaleWithDistace;
    [HideInInspector]
    public float debugWindowWidth = 0.4f;
    [HideInInspector]
    public float debugWindowHeight = 0.3f;

    [HideInInspector]
    Dictionary<GameObject, Dictionary<string, object>> _stringDict = new Dictionary<GameObject, Dictionary<string, object>>();
    [HideInInspector]
    Vector2 temp;

    public static Dictionary<GameObject, Dictionary<string, object>> StringDict { get { if (_instance != null) return _instance._stringDict; else return null; } }
    static UtilOnGUI _instance;
    //Log Tag Filter
    [HideInInspector]
    List<string> logTagNames = new List<string>();
    [HideInInspector]
    Dictionary<string, bool> logTagFilterDict = new Dictionary<string, bool>();


    [HideInInspector]
    public static float deltaTime;

    float _currentTime;
    float _oldTime;

    private List<Util.Debug._DebugLine> removeDebugLines = new List<Util.Debug._DebugLine>();
    private GUISkin skin;
    private GUIStyle debugLine_GUIstyle;
    private GUIStyle debugObject_GUIstyle;
    private GUIStyle debugObjectLabel_GUIstyle;
    private Texture arrow;

    void Awake()
    {
        if (!Util.Settings.enabled)
            DestroyImmediate(gameObject);
        else
            _instance = this;
    }

    void OnDestroy()
    {
        _instance = null;
    }

    void OnEnable()
    {
        Application.RegisterLogCallback(HandleLog);
    }
    void OnDisable()
    {
        Application.RegisterLogCallback(null);
    }
    void HandleLog(string logString, string stackTrace, LogType type)
    {
        Color c = Color.red;
        if (type == LogType.Warning || type == LogType.Assert)
            c = Color.yellow;
        else if (type == LogType.Log)
            c = Color.cyan;//new Color(0.8f, 1f, 1f);
        Util.Debug.Log(logString, c, UtilLogTag.Unity);
    }

    void Start()
    {
        arrow = (Texture)Resources.Load("GUISkin/arrow", typeof(Texture));
        foreach (string s in System.Enum.GetNames(typeof(UtilLogTag)))
        {
            logTagNames.Add(s);
            //TODO: These should probably be made persistent between "plays"
            logTagFilterDict.Add(s, true);
        }
    }

    void Init()
    {
        skin = ((GUISkin)Resources.Load("UtilGUISkin"));
        debugObject_GUIstyle = skin.box;
        debugObject_GUIstyle.alignment = TextAnchor.MiddleLeft;
        debugObject_GUIstyle.font = (Font)Resources.Load("Courier");
        debugObject_GUIstyle.normal.textColor = objectTextColor;

        debugObjectLabel_GUIstyle = skin.label;
        debugObjectLabel_GUIstyle.alignment = TextAnchor.MiddleLeft;
        debugObjectLabel_GUIstyle.font = (Font)Resources.Load("Courier");
        debugObjectLabel_GUIstyle.normal.textColor = objectTextColor;

        debugLine_GUIstyle = GUIStyle.none;
        debugLine_GUIstyle.font = skin.font;

    }

    void OnGUI()
    {
        if (debugLine_GUIstyle == null || debugObject_GUIstyle == null)
        {
            Init();
        }

        if (onGUI)
        {
            DrawDebugLines();
            DrawDebugLinesPersistent();
        }

        if (onGUIObjects)
        {
            //Debug On GUI
            DrawOnObject();
        }
        RemoveExpiredDebugLines();
    }

    void UpdateDeltaTime()
    {
        _currentTime = Time.realtimeSinceStartup;
        deltaTime = _currentTime - _oldTime;
        _oldTime = _currentTime;

        Util.SetDeltaTime(deltaTime);
    }

    void LogData()
    {
        /*string path = Application.persistentDataPath + "/LogFiles";
        if (!Directory.Exists(path))
        {

            Directory.CreateDirectory(path);
        }

        string log = "";
        foreach (Util.Debug._DebugLine dbl in Util.Debug.DebugLines)
        {
            log += dbl.line + " - [" + dbl.tag + "]\n";
        }
        path += "/" + string.Format("Log " + "{0:yyyy-MM-dd_-_HH-mm-ss}.txt", System.DateTime.Now);
        StreamWriter SSS = File.CreateText(path);
        SSS.WriteLine(log);
        SSS.Close();
        Util.Debug.Log("Log saved to: " + path);*/
    }

    void Update()
    {
        UpdateDeltaTime();
        Util.Debug.UnqueueLog();

        if (onGUIObjects)
        {
            //TODO: Do own sort method. Linq doesn't work on iOS (maybe)
            //Sort debug on object windows by distance
            _stringDict = (from entry in _stringDict
                           orderby Vector3.Distance(entry.Key.transform.position, Camera.main.transform.position) descending
                           select entry).ToDictionary(pair => pair.Key, pair => pair.Value);
        }
    }

    public void AddStringAtPosition(GameObject gameObject, string s, object value)
    {
        if (!_stringDict.ContainsKey(gameObject))
        {
            _stringDict.Add(gameObject, new Dictionary<string, object>());
            _stringDict[gameObject].Add(s, value);
        }
        else
        {
            if (!_stringDict[gameObject].ContainsKey(s))
            {
                _stringDict[gameObject].Add(s, value);
            }
            else
            {
                _stringDict[gameObject][s] = value;
            }
        }
    }

    void DrawOnObject()
    {
        foreach (GameObject go in _stringDict.Keys)
        {
            Vector3 drawPos = go.transform.position + new Vector3(0, go.transform.localScale.y, 0);
            Vector3 pos = Camera.main.WorldToScreenPoint(drawPos);
            if (pos.z > 0)
            {
                Dictionary<string, object> dic = _stringDict[go];
                int longestString = 0;
                int counter = 1;
                string s = "";
                float distance = 1;

                if (scaleWithDistace)
                {
                    distance = (Vector3.Distance(Camera.main.transform.position, drawPos)) / 4;
                }

                foreach (KeyValuePair<string, object> kvp in dic)
                {
                    string ss = kvp.Key + kvp.Value;
                    s += ss;
                    if (ss.Length > longestString)
                        longestString = ss.Length;
                    if (counter != dic.Count)
                    {
                        counter++;
                        s += "\n";
                    }
                }

                pos.y = Screen.height - pos.y;

                int fontSize = (int)(objectTextSize / distance);


                if (objectTextBackground)
                {
                    //debugObject_GUIstyle.normal.background = bg;
                    debugObject_GUIstyle.fontSize = fontSize;
                    debugObject_GUIstyle.normal.textColor = objectTextColor;
                    GUI.Box(new Rect(pos.x - 6, pos.y - 12, (longestString * (fontSize * 0.65f)) + 12, fontSize * dic.Count + 10), s, debugObject_GUIstyle);
                    debugObject_GUIstyle.fontSize = textSize;
                }
                else
                {
                    debugObjectLabel_GUIstyle.fontSize = fontSize;
                    debugObjectLabel_GUIstyle.normal.textColor = objectTextColor;
                    GUI.Label(new Rect(pos.x, pos.y, (longestString * fontSize * 0.65f) + 12, fontSize * dic.Count + 12), s, debugObjectLabel_GUIstyle);
                    debugObjectLabel_GUIstyle.fontSize = textSize;
                }
            }
        }
    }

    private Vector2 internal_debugLinesScrollPosition = Vector2.zero;
    private float internal_previousNumOfLines = 0;

    private Vector2 internal_logTagFilterScrollPosition = Vector2.zero;
    static bool toogleLabels = false;
    static bool toogleLines = true;

    void DrawDebugLines()
    {
        GUI.skin = skin;
        float btnHeight = Mathf.Clamp(Screen.height * 0.05f, 32, 200);
        //if(textBackground && Util.DebugLines.Count > 0) //we would want this always, so that we can scroll up and down
        if (Util.Debug.DebugLines.Count > 0 && toogleLines)
        {
            //Log Tag filter
            float swWidth = Screen.width * debugWindowWidth;
            float swHeight = Screen.height * debugWindowHeight;


            float swWidth2 = Screen.width * debugWindowWidth;
            float swHeight2 = Screen.height * debugWindowHeight;

            //Rect groupRect = new Rect(5, Screen.height - swHeight - ltfHeight - 6 - 6, swWidth, ltfHeight);
            //Rect boxRect = new Rect(2, Screen.height - swHeight - ltfHeight - 9 - 9, swWidth + 6, ltfHeight + 6 + 6); //Box is above

            Rect groupRect = new Rect(10 + swWidth, Screen.height - swHeight - 6, swWidth / 3, swHeight2 - 3);
            Rect boxRect = new Rect(10 + swWidth, Screen.height - swHeight - 9, swWidth / 3, swHeight2 + 5); // Box is to the right

            if (toogleLabels)
            {
                GUI.Box(boxRect, "", GUI.skin.box);
                GUILayout.BeginArea(groupRect);
                internal_logTagFilterScrollPosition = GUILayout.BeginScrollView
                (
                    internal_logTagFilterScrollPosition//, false, true, GUILayout.Width(swWidth), GUILayout.Height(ltfHeight)
                );
                //we are drawing everything inside a scrollview
                debugLine_GUIstyle.normal.textColor = textColor;
                debugLine_GUIstyle.margin = new RectOffset(0, 0, 0, 0);
                debugLine_GUIstyle.border = new RectOffset(0, 0, 0, 0);
                debugLine_GUIstyle.fontSize = textSize;

                foreach (string tagName in logTagNames)
                {
                    logTagFilterDict[tagName] = GUILayout.Toggle(logTagFilterDict[tagName], tagName);//, GUILayout.Width(swWidth * 0.9f));
                }
                GUILayout.EndScrollView();
                GUILayout.EndArea();
            }

            //Debug lines
            if (internal_previousNumOfLines != Util.Debug.DebugLines.Count && GUIUtility.hotControl == 0)
            {
                internal_debugLinesScrollPosition.y = Mathf.Infinity;
            }
            internal_previousNumOfLines = Util.Debug.DebugLines.Count;


            Rect groupRect2 = new Rect(5, Screen.height - swHeight2 - 6, swWidth2, swHeight2);
            Rect boxRect2 = new Rect(2, Screen.height - swHeight2 - 9, swWidth2 + 6, swHeight2 + 6);
            if (textBackground)
                GUI.Box(boxRect2, "");

            GUILayout.BeginArea(groupRect2);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Close", GUILayout.Height(btnHeight)))
            {
                toogleLines = false;
            }

            if (GUILayout.Button(toogleLabels ? "Hide Tags" : "Show Tags", GUILayout.Height(btnHeight)))
            {
                toogleLabels = !toogleLabels;
            }
            if (GUILayout.Button("Save Log", GUILayout.Height(btnHeight)))
            {
                LogData();
            }
            if (GUILayout.Button("Clear Log", GUILayout.Height(btnHeight)))
            {
                Util.Debug.ClearLog();
                Util.Debug.Log("Log Cleared", 5);
            }
            GUILayout.EndHorizontal();
            //				GUI.SetNextControlName("DebugLinesScrollView");
            internal_debugLinesScrollPosition = GUILayout.BeginScrollView
            (
                internal_debugLinesScrollPosition
            );

            //we are drawing everything inside a scrollview
            debugLine_GUIstyle.normal.textColor = textColor;
            debugLine_GUIstyle.margin = new RectOffset(0, 0, 0, 0);
            debugLine_GUIstyle.border = new RectOffset(0, 0, 0, 0);
            debugLine_GUIstyle.fontSize = textSize;
            string _tagName = "";
            bool prev_richText = debugLine_GUIstyle.richText;
            Color tempC;

            foreach (Util.Debug._DebugLine dbl in Util.Debug.DebugLines)
            {
                _tagName = dbl.tag.ToString();
                if (logTagFilterDict.ContainsKey(_tagName) && !logTagFilterDict[_tagName])
                    continue;

                if (dbl.expirationTime <= Time.time)
                    removeDebugLines.Add(dbl);

                debugLine_GUIstyle.richText = true;

                tempC = UtilLogTagColors._Color[Mathf.Max(0, (int)Mathf.Log((int)dbl.tag, 2))];

                //					AdvancedLabel.DrawLayout( string.Format("{0} [{1}] ({2})", dbl.line, tag.ToString(), string.Format("{0:0.000}", dbl.creationTime)));

                AdvancedLabel.DrawLayout(string.Format("<color={0}>{1}</color> <color={2}>[{3}]</color> <color={4}>({5})</color>",
                dbl.color.ToHex(), //line color
                dbl.line, //line
                tempC.ToHex(), //line color, //tag color
                _tagName, //tag
                "white", //timestamp color
                string.Format("{0:0.000}", dbl.creationTime)), debugLine_GUIstyle);

                //					GUILayout.Label(string.Format("<color={0}>{1}</color> <color={2}>[{3}]</color> <color={4}>({5})</color>", 
                //					"#" + Mathf.FloorToInt(dbl.color.r*255).ToString("X2") + Mathf.FloorToInt(dbl.color.g*255).ToString("X2") + Mathf.FloorToInt(dbl.color.b*255).ToString("X2"), //line color
                //					dbl.line, //line
                //					"#" + Mathf.FloorToInt(tempC.r*255).ToString("X2") + Mathf.FloorToInt(tempC.g*255).ToString("X2") + Mathf.FloorToInt(tempC.b*255).ToString("X2"), //line color, //tag color
                //					_tagName, //tag
                //					"white", //timestamp color
                //					string.Format("{0:0.000}", dbl.creationTime)), //timestamp
                //					debugLine_GUIstyle);
            }

            //				StringBuilder _stringer = new StringBuilder();
            //			
            //				foreach (Util.Debug._DebugLine dbl in Util.Debug.DebugLines)
            //			    {
            //					_tagName = dbl.tag.ToString();
            //					if(!logTagFilterDict[_tagName])
            //						continue;
            //				
            //					if(dbl.expirationTime <= Time.time)
            //						removeDebugLines.Add(dbl);
            //					
            //					debugLine_GUIstyle.richText = true;
            //					tempC = UtilLogTagColors._Color[ (int) dbl.tag ];
            //					
            //					_stringer.AppendLine
            //					( 
            //						string.Format("<color={0}>{1}</color> <color={2}>[{3}]</color> <color={4}>({5})</color>", 
            //						"#" + Mathf.FloorToInt(dbl.color.r*255).ToString("X2") + Mathf.FloorToInt(dbl.color.g*255).ToString("X2") + Mathf.FloorToInt(dbl.color.b*255).ToString("X2"), //line color
            //						dbl.line, //line
            //						"#" + Mathf.FloorToInt(tempC.r*255).ToString("X2") + Mathf.FloorToInt(tempC.g*255).ToString("X2") + Mathf.FloorToInt(tempC.b*255).ToString("X2"), //line color, //tag color
            //						_tagName, //tag
            //						"white", //timestamp color
            //						string.Format("{0:0.000}", dbl.creationTime))
            //					);					
            //			    } 
            //				
            //				AdvancedLabel.DrawLayout( _stringer.ToString(), debugLine_GUIstyle );
            debugLine_GUIstyle.richText = prev_richText;
            GUILayout.EndScrollView();

            GUILayout.EndArea();
        }
        else
        {
            if (GUI.Button(new Rect(2, Screen.height - btnHeight, btnHeight, btnHeight), arrow))
            {
                toogleLines = true;
            }
        }
    }

    void DrawDebugLinesPersistent()
    {
        float x = 10; float y = 9;
        debugLine_GUIstyle.fontSize = textSize;
        debugLine_GUIstyle.normal.textColor = textColor;
        foreach (Util.Debug._DebugLine dbl in Util.Debug.DebugLinesPersistent)
        {
            Rect rect = GUILayoutUtility.GetRect(new GUIContent(string.Format("{0} {1} ({2})", dbl.line, dbl.value.ToString(), string.Format("{0:0.000}", dbl.creationTime))), debugLine_GUIstyle);
            rect.x = x; rect.y = y; rect.width = rect.width + 4;
            if (textBackground && Util.Debug.DebugLinesPersistent.Count > 0)
            {
                GUI.Box(rect, "");
            }

            rect.x = x + 2;
            if (dbl.color != Color.black)
            {
                debugLine_GUIstyle.normal.textColor = dbl.color;
                GUI.Label(rect, string.Format("{0} {1} ({2})", dbl.line, dbl.value.ToString(), string.Format("{0:0.000}", dbl.creationTime)), debugLine_GUIstyle);
                debugLine_GUIstyle.normal.textColor = textColor;
            }
            else
            {
                GUI.Label(rect, string.Format("{0} {1} ({2})", dbl.line, dbl.value.ToString(), string.Format("{0:0.000}", dbl.creationTime)), debugLine_GUIstyle);
            }

            y += textSize * 1.35f;
        }
    }

    void RemoveExpiredDebugLines()
    {
        if (removeDebugLines.Count > 0)
        {
            foreach (Util.Debug._DebugLine dbl in removeDebugLines)
            {
                Util.Debug.DebugLines.Remove(dbl);
            }
            removeDebugLines.Clear();
        }
    }
}
