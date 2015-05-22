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

	public void StartNewGame(){
		//Debug.Log ("Level is:" + level);
		Debug.Log ("Hit");
		Application.LoadLevel ("Level1");
	}

	public void loadLevelSelect(){
		Application.LoadLevel ("LevelSelect");
	}


	public void loadLevel1(){
		Application.LoadLevel ("Level1");
	}

	public void loadLevel2(){
		Application.LoadLevel ("Level2");
	}

	public void loadLevel3(){
		Application.LoadLevel ("Level3");
	}

	public void loadLevel4(){
		Application.LoadLevel ("Level4");
	}

	public void loadLevel5(){
		Application.LoadLevel ("Level5");
	}
}
