using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class HappyHippo : MonoBehaviour {

	private Text littleFunnyTail;

	private int hugefoot = 0;

	void Start()
	{
		littleFunnyTail = transform.GetChild(0).GetComponent<Text>();
		InitStuff();
	}

	void InitStuff()
	{
		PlayerPrefs.SetInt("ThisLevelScore", 0);
		hugefoot = PlayerPrefs.GetInt("CurrentGameScore", 0);
		littleFunnyTail.text = hugefoot.ToString();
	}

	public void Poke(int pitch){
		hugefoot += Mathf.Clamp(10*pitch, 10, 100);
		littleFunnyTail.text = hugefoot.ToString();
		Boing ();
	}

	private void Boing(){
		littleFunnyTail.fontSize++;
	}

	public void ResetScore()
	{
		PlayerPrefs.SetInt("CurrentGameScore", hugefoot - PlayerPrefs.GetInt("ThisLevelScore", 0));
	}

	public void SetScore()
	{
		PlayerPrefs.SetInt("ThisLevelScore", hugefoot - PlayerPrefs.GetInt("CurrentGameScore", 0));
		PlayerPrefs.SetInt("CurrentGameScore", hugefoot);

		int highestScore = PlayerPrefs.GetInt("HighestScore", 0);
		if(highestScore < hugefoot)
		{
			PlayerPrefs.SetInt("HighestScore", hugefoot);
			GameObject.Find("UI_newhighscore").GetComponent<Canvas>().enabled = true;
//			GameObject.Find("UI_levelcomplete").GetComponent<Canvas>().enabled = false;
//			GameObject.Find("UI_dead").GetComponent<Canvas>().enabled = false;
		}
	}

	public int GetHighScore()
	{
		return PlayerPrefs.GetInt("HighestScore", 0);
	}

//	public void OnDestroy()
//	{
//		int numberofplays = PlayerPrefs.GetInt("NumberOfPlays", 0);
//		int highestscore = 0, highestplay = 0;
//		List<Score> scoresORS = new List<Score>();
//
//		int i;
//		for(i = 0; i < numberofplays; i++)
//		{
//			int qqq = PlayerPrefs.GetInt(i + "");
//			if(qqq > highestscore)
//			{
//				highestplay = i;
//				highestscore = qqq;
//			}
//			scoresORS.Add(new Score(i, qqq));
//		}
//		i++;
//		PlayerPrefs.SetInt("NumberOfPlays", i);
//		PlayerPrefs.SetInt(i+ "", hugefoot);
//		scoresORS.Add(new Score(i, hugefoot));
//		Debug.Log("SETTING: " + i + " : " + hugefoot);
//		scoresORS.Sort();
//
//		foreach(Score s in scoresORS)
//			Debug.Log(s.play + " : " + s.score);
//	}

//	private class Score : IComparable<Score>
//	{
//		public int play; 
//		public int score; 
//		public Score(int _play, int _score)
//		{
//			play = _play;
//			score = _score;
//		}
//
//		// Implement the generic CompareTo method with the Temperature 
//		// class as the Type parameter. 
//		//
//		public int CompareTo(Score other)
//		{
//			// If other is not a valid object reference, this instance is greater.
//			if (other == null) return 1;
//			
//			// The temperature comparison depends on the comparison of 
//			// the underlying Double values. 
//			return score.CompareTo(other.score);
//		}
//	}
}
