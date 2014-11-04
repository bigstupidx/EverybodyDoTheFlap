using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EverybodysDead : AbstractArtState {

	public override void OnBirth(){
		//Time.timeScale = 0f;
		GameObject.Find("TheOtherStuffYouSometimesSee").GetComponent<Canvas>().enabled = true;
		GameObject.Find ("ScoreNumber").GetComponent<Text> ().text = GameObject.Find ("Textermucha").GetComponent<Text> ().text;
        DisableAllMovement();
        CameraShake.Shake(0.15f, 100000);

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
