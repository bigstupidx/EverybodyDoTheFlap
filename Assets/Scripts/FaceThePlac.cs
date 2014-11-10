using UnityEngine;
using System.Collections;

public class FaceThePlac : MonoBehaviour 
{
	Transform pacman;

	void Start () 
	{
		pacman = GameObject.Find("PacMan").transform;
	}
	
	void Update () 
	{
		transform.forward = Vector3.Lerp(transform.forward, (pacman.position - transform.position).normalized, 0.14f * Time.deltaTime);
		
	}
}
