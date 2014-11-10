using UnityEngine;
using System.Collections;

public class Muncher : MonoBehaviour 
{
	public void OnTriggerEnter(Collider otter)
	{
		if(otter.GetComponent<PickUp>())
			otter.GetComponent<PickUp>().Pickup();
	}
}
