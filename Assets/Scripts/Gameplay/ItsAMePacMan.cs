using UnityEngine;
using System.Collections;

public class ItsAMePacMan : MonoBehaviour {

    public float massacreDuration = 7;
    public static bool MurderMode = false;
    static ItsAMePacMan instance;

    float murderTime = 0;

    void Start()
    {
        instance = this;
    }
	
	void Update () {
	    if (MurderMode)
        {
            murderTime -= Time.deltaTime;
            if (murderTime <= 0)
                OHCRAPIMFUCKED();
        }
	}

    public static void YEAHMOTHERFUCKER()
    {
        MurderMode = true;
        instance.murderTime = instance.massacreDuration;
        AudioPlayer.instance.EnterHighPass();
        instance.StopAllCoroutines();
        foreach (MoveThePac move in GameObject.FindObjectsOfType<MoveThePac>())
        {
            if (move.isPacman)
            {
                move.GetComponentInChildren<TrailRenderer>().time = 2;
            }
            else
            {
                move.GetComponentInChildren<TrailRenderer>().time = 1;
            }
        }
    }

    public static void OHCRAPIMFUCKED()
    {
        MurderMode = false;
        instance.murderTime = 0;
        AudioPlayer.instance.ExitHighPass();
        foreach (MoveThePac move in GameObject.FindObjectsOfType<MoveThePac>())
        {
            if (move.isPacman)
            {
                move.GetComponentInChildren<TrailRenderer>().time = 0.4f;
            }
            else
            {
                move.GetComponentInChildren<TrailRenderer>().time = 0.25f;
            }
        }
    }

}
