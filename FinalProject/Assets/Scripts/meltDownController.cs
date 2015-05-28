using UnityEngine;
using System.Collections;

public class meltDownController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine (Meltdown ()); 
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	
	public IEnumerator Meltdown(){
		yield return new WaitForSeconds (8f);
		Debug.Log ("We Waited");
		Application.LoadLevel ("Level1");
	}
}
