using UnityEngine;
using System.Collections;

public class BigPellet : PickUp
{
    public GameObject particle;
    public override void Pickup()
    {
        AudioPlayer.instance.PowerUp();
        
        //CFX_SpawnSystem.GetNextObject(particle, true).transform.position = transform.position;
        ItsAMePacMan.YEAHMOTHERFUCKER();

		if (Time.timeScale == 0f)
			return;
		
		Pellet.pelletsAmount--;
		if (Pellet.pelletsAmount <= 0)
			GameObject.Find ("Illuminati").SendMessage ("DuckfestComplete");

		Destroy(gameObject);
    }


	void Start(){
		Pellet.pelletsAmount++;
	}
}
