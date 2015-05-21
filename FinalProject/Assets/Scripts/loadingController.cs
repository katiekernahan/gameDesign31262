using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class loadingController : MonoBehaviour {

	public Button levelButton;

	// Use this for initialization
	void Start () {
		levelButton = GameObject.Find ("LevelButton").GetComponent<Button>();
		//Invoke ("LoadLevel", 3f);
		if(playerController.level != 1)
			levelButton.interactable = true;

		Debug.Log ("Level is:" + playerController.level);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void LoadLevel(){
		//Debug.Log ("Level is:" + level);
		Debug.Log ("Hit");
		Application.LoadLevel ("Level1");
	}
}
