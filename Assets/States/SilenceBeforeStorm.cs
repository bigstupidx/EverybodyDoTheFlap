using UnityEngine;
using System.Collections;

public class SilenceBeforeStorm : AbstractArtState {
	
	public override void OnBirth(){
		bBirthed = true;
		Time.timeScale = 0f;
		GameObject.Find("TheStuffYouSeeFirst").GetComponent<Canvas>().enabled = true;
	}

	public override void OnExtermination(){
		GameObject.Find("TheStuffYouSeeFirst").GetComponent<Canvas>().enabled = false;
	}
}
