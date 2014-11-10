using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EverybodysDead : AbstractArtState {

	public override void OnBirth(){
		bBirthed = true;
		//Time.timeScale = 0f;
		GameObject.Find("UI_dead").GetComponent<Canvas>().enabled = true;
		GameObject.Find ("ScoreNumber").GetComponent<Text> ().text = GameObject.Find ("Textermucha").GetComponent<Text> ().text;
        DisableAllMovement();
        CameraShake.Shake(0.15f, 100000);

		GameObject.Find("ScoreLabel").GetComponent<HappyHippo>().SetScore();
		int highASfak = GameObject.Find("ScoreLabel").GetComponent<HappyHippo>().GetHighScore();
		GameObject.Find ("HighScoreNumber").GetComponent<Text>().text = highASfak.ToString();

		GameObject.Find("ScoreLabel").GetComponent<HappyHippo>().ResetScore();
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
