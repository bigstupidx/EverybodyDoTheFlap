using UnityEngine;
using System.Collections;

public class GetMeDown : MonoBehaviour 
{
	public Transform getmeDownObj, getmeDownObj2; 

	public void GoDown(float timeToGo)
	{
		StartCoroutine("GetMeDowner", timeToGo);
	}

	IEnumerator GetMeDowner(float ddd)
	{
		float ttt = ddd;
		Vector3 sss = transform.position, eee = transform.rotation.eulerAngles; 
		while(ddd > 0)
		{
			ddd -= Util.deltaTime;
			transform.position = Vector3.Lerp(getmeDownObj2.position, sss, ddd/ttt);
			transform.rotation = Quaternion.Euler(Vector3.Lerp(getmeDownObj2.transform.rotation.eulerAngles, eee, ddd/ttt));
			yield return null;
		}
		yield return null;
	}
}
