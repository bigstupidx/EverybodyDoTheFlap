using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CallFunctionOnIlluminati : MonoBehaviour 
{
	public string function;

	void Start () 
	{
		GetComponent<Button>().onClick.AddListener(delegate{CallFunction();});
	}
	
	void Update () 
	{
	
	}

	public void CallFunction()
	{
		GameObject.Find("Illuminati").GetComponent<SecretContolScript>().SendMessage(function);
	}
}
