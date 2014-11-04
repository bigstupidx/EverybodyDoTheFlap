using UnityEngine;
using System.Collections;

public class MoveThePac : MonoBehaviour 
{
    public bool isPacman = false;
	public KeyCode qqs = KeyCode.Space;
	public float powerAid;
	public float candyCrush;
	public float sagaTime;
	public float gravity = 20f;
    public GameObject impact;

	bool pushingTheFlap = false;

	float yTank = 0f;
	float flapTimer = float.NegativeInfinity;

	bool doingTheFlap = false;
	bool grounded = true;

	void Update()
	{
		if(Input.anyKeyDown && grounded && Time.timeScale > 0)
		{
			pushingTheFlap = true;
            CFX_SpawnSystem.GetNextObject(impact).transform.position = transform.position;
            if (isPacman)
                AudioPlayer.instance.Jump();
		}
	}

	void FixedUpdate()
	{
//		GetComponent<CharacterController>().Move(transform.forward * Time.deltaTime * powerAid);
		DoIt();
		EverybodyDoTheFlap();
		Grounding();
	}

	void Grounding()
	{
        bool wasGrounded = grounded;
		grounded = false;
		if(flapTimer < Time.time)
		{
			RaycastHit[] raycastshit = Physics.RaycastAll(transform.position, Vector3.down, 0.52f);
			foreach(RaycastHit shiet in raycastshit)
			{
				if(shiet.transform.name == "Cube")
				{
                    if (!wasGrounded)
                    {
                        CFX_SpawnSystem.GetNextObject(impact).transform.position = transform.position;
                        if (isPacman)
                            AudioPlayer.instance.Land();
                    }
					grounded = true;
				}
			}
		}

		if(!grounded)
		{
			yTank -= Time.deltaTime * gravity;
		}
		else 
		{
			yTank = 0;
		}

		transform.position += transform.forward * Time.deltaTime * powerAid -(Vector3.down * yTank * Time.deltaTime);
	}

	void DoIt()
	{
		if(flapTimer < Time.time)
		{
			RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, candyCrush);
			foreach(RaycastHit hit in hits)
			{
				if(hit.transform.name == "Cube")
				{
	//				magicDonkey = FindNewDonkey();
					transform.LookAt(transform.position + FindNewDonkey());
					if(transform.forward == Vector3.right || transform.forward == -Vector3.right)
					{
						Vector3 boogieTime = transform.position;
						boogieTime.z = Mathf.RoundToInt(boogieTime.z);
						transform.position = boogieTime;
					}
					else if(transform.forward == Vector3.forward || transform.forward == -Vector3.forward)
					{
						Vector3 boogieTime = transform.position;
						boogieTime.x = Mathf.RoundToInt(boogieTime.x);
						transform.position = boogieTime;
					}
				}
			}
		}
	}

	Vector3 FindNewDonkey()
	{
		RaycastHit[] hits1 = Physics.RaycastAll(transform.position, transform.right, candyCrush);
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


		RaycastHit[] hits2 = Physics.RaycastAll(transform.position, transform.forward, candyCrush);
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

		RaycastHit[] hits3 = Physics.RaycastAll(transform.position, -transform.right, candyCrush);
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

		RaycastHit[] hits4 = Physics.RaycastAll(transform.position, -transform.forward, candyCrush);
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

	public void MakeMeGoThere(Transform there)
	{
        GameObject oldTrail = GetComponentInChildren<TrailRenderer>().gameObject;
        oldTrail.transform.parent = null;
		Vector3 diller;// = Vector3.zero;
		if(transform.forward == Vector3.right || transform.forward == -Vector3.right)
		{
			diller = there.position + there.forward * 1.5f;
			diller.y = transform.position.y;
			diller.z = transform.position.z;
		}
		else //if(transform.forward == Vector3.forward || transform.forward == -Vector3.forward)
		{
			diller = there.position + there.forward * 1.5f;
			diller.y = transform.position.y;
			diller.x = transform.position.x;
		}
		transform.position = diller;
        GameObject newTrail = GameObject.Instantiate(oldTrail);
        newTrail.transform.position = transform.position;
        newTrail.transform.parent = transform;
        Destroy(oldTrail, oldTrail.GetComponent<TrailRenderer>().time);
	}
	
	void EverybodyDoTheFlap()
	{
		//FLAP!
		if(pushingTheFlap && grounded)
		{
			FlapRoutine(15f);
			pushingTheFlap = false;
		}
	}

	/// <summary>
	/// Flaps the routine.
	/// </summary>
	/// <returns>The routine.</returns>
	void FlapRoutine(float sauce)
	{
		yTank = sagaTime * sauce;
		flapTimer = Time.time + 0.25f;
		grounded = false;
	}

	public void ChangeDonkey()
	{
		transform.LookAt(transform.position + FindNewDonkey());
		if(transform.forward == Vector3.right || transform.forward == -Vector3.right)
		{
			Vector3 boogieTime = transform.position;
			boogieTime.z = Mathf.RoundToInt(boogieTime.z);
			transform.position = boogieTime;
		}
		else if(transform.forward == Vector3.forward || transform.forward == -Vector3.forward)
		{
			Vector3 boogieTime = transform.position;
			boogieTime.x = Mathf.RoundToInt(boogieTime.x);
			transform.position = boogieTime;
		}
	}
}
