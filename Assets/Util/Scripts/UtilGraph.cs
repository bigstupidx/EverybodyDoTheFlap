using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UtilTuple;
using System.Linq;

public class UtilGraph : MonoBehaviour
{
    public static bool DrawGraphs = true;
    Material m;
    public static Vector2 position = new Vector2(10, 10);
    public static int width = 350;
    public static int height = 100;
    public static float updateTime = 0;
    public bool update = true;
    float updateTimer;
    static UtilGraph _graph;

    float _minValue = -1;
    float _maxValue = 1;

    float _minBounds = -1;
    float _maxBounds = 1;

    List<Tuple<Rect, float, string, Color>> _val;
    string _maxNumber;
    string _minNumber;
    Color[] colors = { Color.white, Color.yellow, Color.green, Color.magenta, Color.blue, Color.cyan, Color.grey, Color.black };

    //                          <value,time since startup>
    Dictionary<string, List<Tuple<float, float>>> _plotPoints;

    void Awake()
    {
        m = new Material("Shader \"Lines/Colored Blended\" {" +
                       "SubShader {Tags { \"RenderType\"=\"Overdraw\" } Pass { " +
                 "ZWrite Off ZTest Always Cull Off Fog { Mode Off } " +
                       "BindChannels {" +
                       "Bind \"vertex\", vertex Bind \"color\", color }" +
                       "} } }");
        _graph = this;
        _plotPoints = new Dictionary<string, List<Tuple<float, float>>>();
        updateTimer = updateTime;
    }

    void Update()
    {
        if (DrawGraphs)
        {
            updateTimer -= Util.deltaTime;
            if (updateTimer <= 0)
            {
                update = true;
                updateTimer += updateTime;
                UpdateMinMax();
            }
            else
            {
                update = false;
            }
        }
    }

    void DrawZero()
    {
        if (_maxBounds > 0 && _minValue < 0)
        {
            GL.Color(Color.black);
            Vector2 prevPoint = new Vector2(position.x, Mathf.InverseLerp(_minBounds, _maxBounds, 0) * height);
            GL.Vertex3(prevPoint.x, Screen.height - height - position.y + prevPoint.y, 0);
            GL.Vertex3(prevPoint.x + width, Screen.height - height - position.y + prevPoint.y, 0);
        }
    }

    //For GOD'S SAKE: Remember that Lower-Left corner is (0,0)
    void OnPostRender()
    {
        if (DrawGraphs)
        {
            GL.PushMatrix();
            m.SetPass(0);
            GL.LoadPixelMatrix();
            GL.Begin(GL.LINES);

            DrawZero();
            DrawOutlineBox();

            int color = 0;
            _val = new List<Tuple<Rect, float, string, Color>>();
            if (_plotPoints.Count <= 0)
                return;
            foreach (string line in _plotPoints.Keys)
            {
                DrawLine(_plotPoints[line], line, colors[color]);
                color = (color + 1) % colors.Length;
            }

            GL.End();
            GL.PopMatrix();
        }
    }

    public static void AddPlotData(string name, float data)
    {
        if (_graph && _graph.update)
        {
            if (!_graph._plotPoints.ContainsKey(name))
                _graph._plotPoints.Add(name, new List<Tuple<float, float>>());

            _graph._plotPoints[name].Add(new Tuple<float, float>(data, Time.realtimeSinceStartup));
        }
        else if (!_graph)
        {
            Instantiate(Resources.Load("UtilGraph"));
        }
    }

    public static void ChangeOutlineSize(Rect rect)
    {
        if (_graph)
        {
            position = new Vector2(rect.x, rect.y);
            width = (int)rect.width;
            height = (int)rect.height;
        }
    }

    void DrawLine(List<Tuple<float, float>> line, string name, Color c)
    {
        /*line = (from entry in line orderby entry.GetSecond() descending 
                select entry).ToList();*/
        if (line.Count <= 0)
            return;
        Vector2 prevPoint = new Vector2(width + position.x - ((Time.realtimeSinceStartup - line[0].GetSecond()) * 100), Mathf.InverseLerp(_minBounds, _maxBounds, line[0].GetFirst()) * height);

        GL.Color(c);
        foreach (Tuple<float, float> plot in line)
        {
            Vector2 point = new Vector2(width + position.x - ((Time.realtimeSinceStartup - plot.GetSecond()) * 100), Mathf.InverseLerp(_minBounds, _maxBounds, plot.GetFirst()) * height);
            GL.Vertex3(prevPoint.x, Screen.height - height - position.y + prevPoint.y, 0);
            GL.Vertex3(point.x, Screen.height + point.y - position.y - height, 0);
            prevPoint = point;
        }

        _val.Add(new Tuple<Rect, float, string, Color>(new Rect(prevPoint.x, height - prevPoint.y, 110, 40), line[line.Count - 1].GetFirst(), name, c));
    }

    void DrawOutlineBox()
    {
        GL.Color(Color.red);
        //Upper Bound
        GL.Vertex3(position.x, Screen.height - position.y, 0);
        GL.Vertex3(position.x + width, Screen.height - position.y, 0);

        //Lower Bound
        GL.Vertex3(position.x, Screen.height - position.y - height, 0);
        GL.Vertex3(position.x + width, Screen.height - position.y - height, 0);

        //Left Bound
        GL.Vertex3(position.x, Screen.height - position.y, 0);
        GL.Vertex3(position.x, Screen.height - position.y - height, 0);


        //Right Bound
        GL.Vertex3(position.x + width, Screen.height - position.y, 0);
        GL.Vertex3(position.x + width, Screen.height - position.y - height, 0);
    }

    void UpdateMinMax()
    {
        float tempMin = float.MaxValue;
        float tempMax = float.MinValue;
        if (_plotPoints.Count <= 0)
            return;
        foreach (List<Tuple<float, float>> line in _plotPoints.Values)
        {
            List<Tuple<float, float>> toRemove = new List<Tuple<float, float>>();
            foreach (Tuple<float, float> plot in line)
            {
                if (width - ((Time.realtimeSinceStartup - plot.GetSecond()) * 100) < 0)
                {
                    toRemove.Add(plot);
                }
                else if (plot.GetFirst() > tempMax)
                    tempMax = plot.GetFirst();

                else if (plot.GetFirst() < tempMin)
                    tempMin = plot.GetFirst();
            }
            foreach (Tuple<float, float> plot in toRemove)
            {
                line.Remove(plot);
            }
        }

        _maxValue = tempMax + 0;
        _minValue = tempMin - 0;
        _minNumber = _minValue + "";
        _maxNumber = _maxValue + "";

        if (_maxValue > _maxBounds)
            _maxBounds = _maxValue;
        else
        {
            _maxBounds = Mathf.Lerp(_maxBounds, _maxValue, Util.deltaTime);
        }

        if (_minValue < _minBounds)
            _minBounds = _minValue;
        else
        {
            _minBounds = Mathf.Lerp(_minBounds, _minValue, Util.deltaTime);
        }
    }

    void OnGUI()
    {
        if (DrawGraphs)
        {
            GUI.Label(new Rect(position.x, position.y, 120, 20), _maxNumber);
            GUI.Label(new Rect(position.x, position.y + height, 120, 20), _minNumber);
            foreach (Tuple<Rect, float, string, Color> tf in _val)
            {
                GUI.contentColor = tf.GetFourth();
                GUI.Label(new Rect(position.x + width, tf.object1.y, tf.object1.width, tf.object1.height), tf.object2 + "\n(" + tf.object3 + ")");
            }
        }
    }
}