using UnityEngine;
using System.Collections;

public class cameraController : MonoBehaviour {

	/*public float speed = 1f;   //speed at which the camera will scroll
	private Vector3 newPosition;

	// Use this for initialization
	void Start () {
		newPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {

		newPosition.x += 0.1732050808F/2*Time.timeScale;
		newPosition.y += 0.1F/2*Time.timeScale;
		transform.position = newPosition;

	} */
		
	public GameObject player;

	void Start(){
		player = GameObject.Find ("player");
	}
		
		// Update is called once per frame
		void Update () {
			
			transform.position = new Vector3(player.transform.position.x, player.transform.position.y, 1);
			
		}
}
