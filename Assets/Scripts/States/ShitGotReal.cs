using UnityEngine;
using System.Collections;

public class ShitGotReal : AbstractArtState {
	
	public override void OnBirth(){
		bBirthed = true;
		Time.timeScale = 0f;
		GameObject.Find("ScoreLabel").GetComponent<Canvas>().enabled = true;
		GameObject.Find("UI_startup").SetActive(false);
		GameObject.FindObjectOfType<SecretContolScript>().StartCoroutine("StartUp");
	}
	public override void OnExtermination(){
		GameObject.Find("ScoreLabel").GetComponent<Canvas>().enabled = false;
        
    }
}
