using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MayhemItIsThen : AbstractArtState {
	
	public override void OnBirth(){
		bBirthed = true;
		//Time.timeScale = 0f;
		//GameObject.Find("TheOtherStuffYouSometimesSee").GetComponent<Canvas>().enabled = true;
		GameObject.Find("BeforeTheNewDay").GetComponent<Canvas>().enabled = true;
		GameObject.Find("BeforeTheNewDay").transform.GetChild(0).gameObject.SetActive(true);
		GameObject.Find ("ScoreNumber").GetComponent<Text> ().text = GameObject.Find ("Textermucha").GetComponent<Text> ().text;
		DisableAllMovement();
		CameraShake.Shake(0.03f, 100000);
		
		GameObject.Find("Square").GetComponent<HappyHippo>().SetScore();
		int highASfak = GameObject.Find("Square").GetComponent<HappyHippo>().GetHighScore();
		GameObject.Find ("HighScoreNumber").GetComponent<Text>().text = highASfak.ToString();
		AudioPlayer.instance.EnterLowPass();
	}
	
	void DisableAllMovement()
	{
		foreach (MoveThePac move in GameObject.FindObjectsOfType<MoveThePac>())
		{
			move.enabled = false;
		}
	}
	
	public override void OnExtermination(){
		GameObject.Find("TheOtherStuffYouSometimesSee").GetComponent<Canvas>().enabled = false;
	}
}