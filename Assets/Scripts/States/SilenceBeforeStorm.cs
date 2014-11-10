using UnityEngine;
using System.Collections;

public class SilenceBeforeStorm : AbstractArtState {
	
	public override void OnBirth(){
		bBirthed = true;
		Time.timeScale = 0f;
		GameObject.Find("UI_startup").GetComponent<Canvas>().enabled = true;
	}

	public override void OnExtermination(){
		GameObject.Find("UI_startup").GetComponent<Canvas>().enabled = false;
	}
}
