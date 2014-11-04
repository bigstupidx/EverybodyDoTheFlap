using UnityEngine;
using System.Collections;

public class Pellet : PickUp 
{
	public static int pelletsAmount = 0;

    public GameObject particle;
	public override void Pickup()
    {
        AudioPlayer.instance.PickUp();
		if (Time.timeScale == 0f)
			return;
		
		Pellet.pelletsAmount--;
		if (Pellet.pelletsAmount <= 0)
			GameObject.Find ("Illuminati").SendMessage ("DuckfestComplete");
        
        CFX_SpawnSystem.GetNextObject(particle, true).transform.position = transform.position;

		Destroy(gameObject);

    }

	void Start(){
	
		Pellet.pelletsAmount++;
	}
}
