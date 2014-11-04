using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GastPickup : PickUp 
{
    public Collider collider;
    public Renderer body;
    Material material;
    public Color defaultColor, murderModeColor = Color.blue;
    float deadTimer = 0;
    bool dead = false;

	float std_speed;
	public MoveThePac myPac;
    public GameObject killParticle;


    void Start()
    {
        material = body.material;
        defaultColor = material.color;
		std_speed = myPac.powerAid;
    }

    void Update()
    {
        if (dead)
        {
            deadTimer -= Time.deltaTime;
            if (deadTimer < 3)
                body.enabled = !body.enabled;
            if (deadTimer <= 0)
            {
                body.enabled = true;
                dead = false;
                collider.enabled = true;
            }
        }

        if (ItsAMePacMan.MurderMode)
		{
			myPac.powerAid = std_speed-1.5f;
            material.color = murderModeColor;
		}
        else
		{
			myPac.powerAid = std_speed;
            material.color = defaultColor;
		}
    }

    public override void Pickup()
    {
        if (ItsAMePacMan.MurderMode)
            NomNomNom();
        else
            IMunchedYourAss();
    }
    
    void NomNomNom()
    {
		myPac.ChangeDonkey();
        collider.enabled = false;
        body.enabled = false;
        deadTimer = 10;
        dead = true;
        CFX_SpawnSystem.GetNextObject(killParticle).transform.position = transform.position;
        AudioPlayer.instance.Explosion();
        CameraShake.Shake(0.1f, 1f);
        //Play SFX of nom
    }

    void IMunchedYourAss()
    {
		GameObject.Find ("The Ant").GetComponent<Text>().text = "You are dead.";
		GameObject.Find ("Illuminati").SendMessage ("DuckfestComplete");
        GameObject cloud = GameObject.Instantiate( (GameObject) Resources.Load("FightParticle"));
        cloud.transform.position = GameObject.Find("PacMan").transform.position;
        AudioPlayer.instance.YouLose();
    }
}
