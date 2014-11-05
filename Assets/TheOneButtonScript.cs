using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TheOneButtonScript : MonoBehaviour 
{

	void Update () 
	{
		if(Input.anyKeyDown)
		{
			GameObject.Find("Illuminati").SendMessage("ANewDayIsComing");
			Object.Destroy(this);
		}
	}
}
