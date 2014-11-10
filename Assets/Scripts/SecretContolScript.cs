using UnityEngine;
using System.Collections;

public class SecretContolScript : MonoBehaviour {

	private static AbstractArtState CurrentStateOfThings = new VastAndColdVoid();
    public static SecretContolScript instance;

    void Awake()
    {
        instance = this;
		if(Application.loadedLevel == 0)
			PlayerPrefs.SetInt("CurrentGameScore", 0);
		PlayerPrefs.SetInt("ThisLevelScore", 0);
    }

	void Start () {
		AudioPlayer.instance.EnterLowPass();
		if (CurrentStateOfThings is VastAndColdVoid)
			EstablishStateOfThingsSir(new SilenceBeforeStorm());
		else if(!CurrentStateOfThings.bBirthed)
			CurrentStateOfThings.OnBirth();
	}

	public void EstablishStateOfThingsSir(AbstractArtState NewlyCameToLifeShit){
		CurrentStateOfThings.OnExtermination();
		CurrentStateOfThings = NewlyCameToLifeShit;
		CurrentStateOfThings.OnBirth();
	}

	public void StartThisLevelNow()	{
		EstablishStateOfThingsSir (new ShitGotReal ());
	}

	public void DuckfestComplete(){
		EstablishStateOfThingsSir (new EverybodysDead ());
	}

	public void LetMayhemInsueOnceMore(){
		EstablishStateOfThingsSir (new MayhemItIsThen ());
		StartCoroutine("StartNextLevelIn", 10f);
	}

	IEnumerator StartNextLevelIn(float dillerbat)
	{
		yield return new WaitForSeconds(dillerbat);
		StartNextLevelNow();
		yield return null;
	}

	private void RestartLevel(){
		CurrentStateOfThings.OnExtermination();
		CurrentStateOfThings = new VastAndColdVoid();
		StartThisLevelNow(Application.loadedLevel);
	}

	private void StartLevelOne(){
		StartThisLevelNow(0);
	}

	private void StartThisLevelNow(int levelId){
		Time.timeScale = 0;
		Pellet.pelletsAmount = 0;
		CurrentStateOfThings = new VastAndColdVoid();
		Application.LoadLevel (levelId);
	}

	private void WeAreLackingControl(){
		GameObject.Find ("ControlsButtOn").SetActive(false);
		GameObject.Find ("Controls").GetComponent<UnityEngine.UI.Text>().enabled = true;
	}

	public void StartNextLevelNow()
	{
		StartThisLevelNow(Application.loadedLevel+1);
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

	public void OnLevelWasLoaded(int levelId)
	{

	}
}
