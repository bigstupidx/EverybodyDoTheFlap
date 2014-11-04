using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class UtilConsoleLog : MonoBehaviour
{
    [HideInInspector]
    [SerializeField]
    public List<Util.Debug._DebugLine> editorLog = new List<Util.Debug._DebugLine>();
}
