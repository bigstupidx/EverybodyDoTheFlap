using UnityEngine;
using System.Collections;

public class AwsumGasdSkhipt : MonoBehaviour {

	private MoveThePac TheMohtalEnemyLol;

	void Start() {
		TheMohtalEnemyLol = GameObject.FindObjectOfType<MoveThePac> ();
	}

	void Update(){
		if (
			(TheMohtalEnemyLol.transform.position - transform.position).magnitude < this.GetComponent<SphereCollider> ().radius
			) 
		{
			YnaYnaYna();
		}

	}

	void YnaYnaYna(){
		//This is left for you, Benjamin, so you can make the awsum explosion.
	}
}
