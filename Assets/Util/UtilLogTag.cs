using System;
using System.Collections.Generic;
using UnityEngine;
//This class is off limits!!!
public class UtilLogTagColors
{
	private static Color[] _color = {new Color(1f,1f,1f,1f),new Color(0.8f,1f,1f,1f),new Color(0.8970588f,0.7169407f,0f,1f),new Color(0.3610868f,0.3285575f,0.7573529f,1f),new Color(0.069002f,0.4402985f,0.2712945f,1f),new Color(1f,0f,0f,1f),new Color(0f,1f,0.9586208f,1f)};
	public static Color[] _Color 
	{
		get{ return _color; }
	}
}
public enum UtilLogTag {Default = 1,Unity = 2,Other = 4,Network = 8,Miscellaneous = 16,Graphics = 32,NewLogTag = 64}