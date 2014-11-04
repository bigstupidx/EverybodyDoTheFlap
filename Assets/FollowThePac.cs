using UnityEngine;
using System.Collections;

public class FollowThePac : MonoBehaviour 
{
	public Transform pac; 

	// Use this for initialization
	void Start () 
	{
		if(pac == null)
		{
			pac = transform.parent;
		}
		transform.parent = null;
	}
	
	void FixedUpdate () 
	{
		transform.position = pac.position;
		transform.forward = pac.forward;
//		float nuller = Mathf.Lerp
//		(
//			transform.position.y, 
//			pac.position.y, 
//			Time.deltaTime * 4f
//		);
//
//		transform.position = new Vector3
//		(
//			pac.position.x, 
//			nuller,
//			pac.position.z
//		);
	}
}
