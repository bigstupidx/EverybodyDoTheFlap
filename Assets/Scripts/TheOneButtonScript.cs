using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TheOneButtonScript : MonoBehaviour 
{

	void Update () 
	{
		if(Input.anyKeyDown)
		{
			Util.Debug.Log("TheOneButtonScript doesn't actually do anything because other stuff was more important at the time of writing.");
			//ExecuteEvents.Execute<IPointerClickHandler>(gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
			//GameObject.Find("Illuminati").SendMessage("ANewDayIsComing");
			Object.Destroy(this);
		}
	}
}
