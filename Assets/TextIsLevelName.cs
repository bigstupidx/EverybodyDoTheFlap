using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextIsLevelName : MonoBehaviour 
{

	void Start () 
	{
		GetComponent<Text>().text = Application.loadedLevelName;
		Object.Destroy(this);
	}

}
