using UnityEngine;
using System.Collections.Generic;

public class FrenchArmyTactics : MonoBehaviour 
{	
	public float checkerX, checkerY;
	
	private GameObject scoreText;

	List<MoveThePac> pacList  =new List<MoveThePac>();

	public float checker = 0.2f;

	private bool checkAgainNow = false;

	void Update(){
		RectTransform a = scoreText.GetComponent<RectTransform> ();
		a.pivot = new Vector2 (checkerX,checkerY);
		a.anchorMax = new Vector2 (checkerX+0.1f,checkerY+0.1f);
		a.anchorMin = new Vector2 (checkerX-0.1f,checkerY-0.1f);
		a.position = Vector3.zero;
		a.anchoredPosition = Vector2.zero;

		if (checkAgainNow)
			return;

		foreach (MoveThePac paccer in pacList) {
			if (InTheWayCheck(paccer)){
				CheckNow();
				return;
			}
		}
    }

	void Start(){
		scoreText = transform.GetChild(0).gameObject;
		checkerX = scoreText.GetComponent<RectTransform> ().pivot.x;
		checkerY = scoreText.GetComponent<RectTransform> ().pivot.y;
		pacList.AddRange(GameObject.FindObjectsOfType<MoveThePac>());


	}

	public void CheckNow(){

		checkAgainNow = true;

		iTween.ValueTo(gameObject, iTween.Hash( "from", checkerX, "to", aa(), "onupdate", "aaa", "time", 0.5f, "easetype", iTween.EaseType.easeOutExpo ));
		iTween.ValueTo(gameObject, iTween.Hash( "from", checkerY, "to", AA(), "onupdate", "AAAA", "time", 0.5f, "easetype", iTween.EaseType.easeOutExpo ));
    
		Invoke ("_aaAAaaAA", 0.5f);
	
	}

	private void _aaAAaaAA(){
		checkAgainNow = false;
	}

	float aa(){
		return Random.Range (0.1f, 0.65f);
	}

	float AA(){
		return Random.Range (0.1f, 0.9f);
    }

	void aaa(float a){
		checkerX = a;
	}
	
	void AAAA(float A){
		checkerY = A;
    }

	public Vector3 checkAgainstMe;
	public Vector3 screenPoint;

	private bool InTheWayCheck(MoveThePac _aa){

		screenPoint = Camera.main.WorldToScreenPoint (_aa.transform.position);

		screenPoint = new Vector3 (
			screenPoint.x / Screen.width,
			screenPoint.y / Screen.height,
			0f);

		checkAgainstMe = new Vector3 (
			checkerX,
			checkerY,
            0f);

		return ((screenPoint - checkAgainstMe).magnitude < checker);
	}
}
