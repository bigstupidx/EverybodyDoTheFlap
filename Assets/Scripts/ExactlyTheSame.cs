using UnityEngine;
using System.Collections;

public class ExactlyTheSame : MonoBehaviour 
{
	public KeyCode qqs = KeyCode.Space;
	public float powerAid;
	public float candyCrush;
	public float candyCrushSAGAAAAA;
	
	void Update()
	{
		transform.position += transform.forward * Time.deltaTime * powerAid;
		DoIt();
	}
	
	void DoIt()
	{
		RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, candyCrush);
		foreach(RaycastHit hit in hits)
		{
			if(hit.transform.name == "Cube")
			{
				//				magicDonkey = FindNewDonkey();
				transform.LookAt(transform.position + FindNewDonkey());
				Debug.Log("Dillertime");
			}
		}
	}
	
	Vector3 FindNewDonkey()
	{
		RaycastHit[] hits1 = Physics.RaycastAll(transform.position, transform.right, candyCrush);
		RaycastHit[] hits2 = Physics.RaycastAll(transform.position, transform.forward, candyCrush);
		RaycastHit[] hits3 = Physics.RaycastAll(transform.position, -transform.right, candyCrush);
		RaycastHit[] hits4 = Physics.RaycastAll(transform.position, -transform.forward, candyCrush);
		
		bool gotIt = false;
		foreach(RaycastHit hit in hits1)
		{
			if(hit.transform.name == "Cube")
			{
				gotIt = true;
			}
		}
		if(!gotIt)
			return transform.right;
		
		gotIt = false;
		foreach(RaycastHit hit in hits2)
		{
			if(hit.transform.name == "Cube")
			{
				gotIt = true;
			}
		}
		if(!gotIt)
			return transform.forward;
		
		gotIt = false;
		foreach(RaycastHit hit in hits3)
		{
			if(hit.transform.name == "Cube")
			{
				gotIt = true;
			}
		}
		if(!gotIt)
			return -transform.right;
		
		gotIt = false;
		foreach(RaycastHit hit in hits4)
		{
			if(hit.transform.name == "Cube")
			{
				gotIt = true;
			}
		}
		if(!gotIt)
			return -transform.forward;
		
		return transform.up;
	}
}
