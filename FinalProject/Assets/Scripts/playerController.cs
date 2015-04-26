using UnityEngine;
using System.Collections;

public class playerController : MonoBehaviour {
	
	//float positionFromCenter = 0;
	// Use this for initialization
	private Rigidbody2D _myRigidBody;
	private bool _jumping;
	private float _distanceFromGround = 0.0f;
	private Vector3 currentPosition;
	private bool _jumpingUp = false;
	
	
	void Start () {
		Time.timeScale = 1.0F;
		_myRigidBody = GetComponent<Rigidbody2D>();
		
	}
	
	
	void jump(){
		if (_jumpingUp) {
			currentPosition.y += 0.1F; 
			_distanceFromGround += 0.1F;
			transform.localScale += new Vector3(0.01F,0.01F,0);
			if (_distanceFromGround > 2)
				_jumpingUp = false;
		} 
		else {
			currentPosition.y -= 0.1F;
			_distanceFromGround -= 0.1F;
			transform.localScale += new Vector3(-0.01F,-0.01F,0);
			if (_distanceFromGround < 0){
				GetComponent<Animator> ().SetBool ("jumping", false);
				_jumping = false;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
		currentPosition = transform.position;
		float move = Input.GetAxisRaw("Horizontal");
		
		if (Input.GetKeyDown ("space")) {
			Debug.Log ("Space hit");
			_jumping = true;
			_jumpingUp = true;
			GetComponent<Animator> ().SetBool ("jumping", true);
		}
		if (_jumping) {
			jump ();
		}
		
		float xMovement = 0.1732050808F/2;   //isometrix values
		float yMovement = 0.1F/2; 
		
		xMovement += move * 0.5f / 2;
		yMovement -= move * 0.5f / 2;
		
		
		
		currentPosition.x += xMovement*Time.timeScale;
		currentPosition.y += yMovement*Time.timeScale;
		
		
		transform.position = currentPosition;
		
	}
	
	void FixedUpdate(){
		//reserved for non rigid/movement operations - this is independent of timescale
	}
	
	void OnTriggerEnter2D( Collider2D other )
	{
		if (other.gameObject.name == "fastCubeCollider"){
			Time.timeScale = 2;
			Object.Destroy (other.gameObject, 0.0F);
			Object.Destroy (GameObject.Find("fastCube"));
			GetComponent<Animator>().SetBool( "runningFast", true );
		}
		else
			Application.LoadLevel("beforeTest");
		
	}
	
	
}

