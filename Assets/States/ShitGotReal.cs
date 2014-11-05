using UnityEngine;
using System.Collections;

public class ShitGotReal : AbstractArtState {
	
	public override void OnBirth(){
		bBirthed = true;
		Time.timeScale = 0f;
		GameObject.Find("Square").GetComponent<Canvas>().enabled = true;
		GameObject.FindObjectOfType<SecretContolScript>().StartCoroutine("StartUp");
	}
	public override void OnExtermination(){
		GameObject.Find("Square").GetComponent<Canvas>().enabled = false;
        
    }
}
