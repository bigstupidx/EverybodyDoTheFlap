using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MayhemItIsThen : AbstractArtState {
	
	public override void OnBirth(){
		bBirthed = true;
		//Time.timeScale = 0f;
		//GameObject.Find("TheOtherStuffYouSometimesSee").GetComponent<Canvas>().enabled = true;
		GameObject.Find("UI_levelcomplete").GetComponent<Canvas>().enabled = true;
		GameObject.Find("UI_levelcomplete").transform.GetChild(0).gameObject.SetActive(true);
		GameObject.Find ("ScoreNumber").GetComponent<Text> ().text = GameObject.Find ("Textermucha").GetComponent<Text> ().text;
		DisableAllMovement();
		CameraShake.Shake(0.03f, 100000);
		
		GameObject.Find("ScoreLabel").GetComponent<HappyHippo>().SetScore();
		int highASfak = GameObject.Find("ScoreLabel").GetComponent<HappyHippo>().GetHighScore();
		GameObject.Find ("HighScoreNumber").GetComponent<Text>().text = highASfak.ToString();
		GameObject.Find ("HighScoreName").GetComponent<Text>().text = PlayerPrefs.GetString("HighScoreName", "no one");
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
		GameObject.Find("UI_dead").GetComponent<Canvas>().enabled = false;
	}
}