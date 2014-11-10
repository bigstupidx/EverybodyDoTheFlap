using UnityEngine;
using System.Collections;

public class Steamer : MonoBehaviour 
{
	public Transform otter;

	public void OnCollisionEnter(Collision pac)
	{
		MoveThePac mover = pac.transform.GetComponent<MoveThePac>();
		if(mover != null)
		{
			mover.MakeMeGoThere(otter);
		}
	}
}
