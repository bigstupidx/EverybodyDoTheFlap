using UnityEngine;
using System.Collections;

public class SecretContolScript : MonoBehaviour {

	private static AbstractArtState CurrentStateOfThings = new VastAndColdVoid();
    public static SecretContolScript instance;

    void Awake()
    {
        instance = this;
    }

	void Start () {
		AudioPlayer.instance.EnterLowPass();
		if (CurrentStateOfThings is VastAndColdVoid)
			EstablishStateOfThingsSir(new SilenceBeforeStorm());
		else 
			CurrentStateOfThings.OnBirth();
	}

	public void EstablishStateOfThingsSir(AbstractArtState NewlyCameToLifeShit){
		CurrentStateOfThings.OnExtermination();
		CurrentStateOfThings = NewlyCameToLifeShit;
		CurrentStateOfThings.OnBirth();
	}

	public void TheWorldShallKnowOurMight()
	{
		EstablishStateOfThingsSir (new ShitGotReal ());
	}

	public void DuckfestComplete(){
		EstablishStateOfThingsSir (new EverybodysDead ());
	}

	private void ThatWasFunLetsDoThatAgain(){
		CurrentStateOfThings.OnExtermination();
		CurrentStateOfThings = new ShitGotReal ();
		Ktch ();
	}

	private void Ktch(){
        Time.timeScale = 0;
		Application.LoadLevel (0);
	}

	private void WeAreLackingControl(){
		GameObject.Find ("ControlsButtOn").SetActive(false);
		GameObject.Find ("Controls").GetComponent<UnityEngine.UI.Text>().enabled = true;
	}

	IEnumerator StartUp()
	{
		AudioPlayer.instance.ExitLowPass();

		float slowTime = 3f;
		float daerm = slowTime;

		GameObject.Find("Main Camera").GetComponent<GetMeDown>().GoDown(daerm);

		while(slowTime > 0)
		{
			slowTime -= Util.deltaTime;
			yield return null;
		}
		Time.timeScale = 1.0f;
		yield return null;
	}
}
