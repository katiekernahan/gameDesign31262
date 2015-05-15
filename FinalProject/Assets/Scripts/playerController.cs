using UnityEngine;
using System.Collections;

public class playerController : MonoBehaviour {

	//SUPERPOWERS
	private float runningSpeed = 1.0f;
	private bool isFlash = false;
	private bool isInvisible = false;
	private bool isHulk = false;
	private bool isSuperMan = false;
	private bool isMutated = false;

	private float isometricAngle =  Mathf.Tan ((26.5f * Mathf.PI)/180);

	//float positionFromCenter = 0;
	// Use this for initialization
	public int sortingOrder = 6;
	private SpriteRenderer sprite;

	private float fractionMovement = 1.0f;

	private Rigidbody2D _myRigidBody;
	private bool _jumping;
	private float _distanceFromGround = 0.0f;
	private Vector3 currentPosition;
	private bool _jumpingUp = false;
	private bool _slidingLeft = false;
	private bool _slidingRight = false;
	private float _distanceFromOriginSlide = 0.0f;

	private GameObject shadow;
	private Vector3 shadowPosition;
	private Vector3 shadowScale;

	private GameObject playerCollider;
	private Vector3 playerColliderPosition;
	
	void Start () {
		Time.timeScale = 1.0F;
		_myRigidBody = GetComponent<Rigidbody2D>();
		shadow = GameObject.Find ("shadow");
		playerCollider = GameObject.Find ("playerCollider");

		sprite = GetComponent<SpriteRenderer>();

	}
	
	
	void jump(){
		shadowPosition = shadow.transform.position;
		shadowScale = shadow.transform.localScale;
		playerColliderPosition = playerCollider.transform.position;
		if (_jumpingUp) {
			shadowPosition.y -= 0.1F;
			shadowScale.y -= 0.02F;
			shadowScale.x -= 0.02F;

			playerColliderPosition.y -= 0.1F;

			currentPosition.y += 0.1F; 
			_distanceFromGround += 0.1F;
			transform.localScale += new Vector3(0.01F,0.01F,0);
			if (_distanceFromGround > 2)
				_jumpingUp = false;
		} 
		else {
			//shadow.transform.position.y += 0.1F;
			shadowPosition.y += 0.1F;
			shadowScale.y += 0.02F;
			shadowScale.x += 0.02F;

			playerColliderPosition.y += 0.1F;

			currentPosition.y -= 0.1F;
			_distanceFromGround -= 0.1F;
			transform.localScale += new Vector3(-0.01F,-0.01F,0);
			if (_distanceFromGround < 0){
				GetComponent<Animator> ().SetBool ("jumping", false);
				_jumping = false;
			}
		}
		shadow.transform.position = shadowPosition;
		shadow.transform.localScale = shadowScale;
		playerCollider.transform.position = playerColliderPosition;
	}

	void giveSuperpower()
	{

	}
	
	//KEEP PLAYER COLLIDER ON GROUND WHEN JUMPING
	
	// Update is called once per frame
	void Update () {
		currentPosition = transform.position;
		//float move = Input.GetAxisRaw("Horizontal");
		float move = 0;


		if(Input.GetKeyDown("left")){
			//move = -5;
			//slideLeft();
			if(sortingOrder != 7 && !_slidingRight && !_slidingLeft){
				_slidingLeft = true;
				sortingOrder++;
				sprite.sortingOrder--;
			}
		}

		if(Input.GetKeyDown("right")){

			if(sortingOrder != 0 && !_slidingLeft && !_slidingRight){
				_slidingRight = true;
				sprite.sortingOrder++;
				sortingOrder--;
			}
			//slideRight();
		}

		
		if (Input.GetKeyDown ("space")) {
			Debug.Log ("Space hit");
			_jumping = true;
			_jumpingUp = true;
			GetComponent<Animator> ().SetBool ("jumping", true);
		}
		if (_jumping) {
			jump ();
		}

		if (isMutated)
			giveSuperpower ();

		//float xMovement = (0.1732050808F/2)*runningSpeed;   //isometrix values
		//float yMovement = (0.1F/2)*runningSpeed; 
		float xMovement = 1 * runningSpeed/10;
		float yMovement = isometricAngle * runningSpeed/10;
		
		if (_slidingLeft) {
			if(_distanceFromOriginSlide < 4.5f){ //fraction to move in between lines
				move -= 0.5f;
				_distanceFromOriginSlide += 0.5f; 
			}
			else{
				_slidingLeft = false;
				_distanceFromOriginSlide = 0;
			}
		} 
		else if (_slidingRight) {
			if(_distanceFromOriginSlide < 4.5f){
				move += 0.5f;
				_distanceFromOriginSlide += 0.5f;
			}
			else {
				_slidingRight = false;
				_distanceFromOriginSlide = 0;
			}
		} 

			xMovement += move * 0.5f / 2;
			yMovement -= move * 0.5f / 2;
			
			
			
			currentPosition.x += xMovement;
			currentPosition.y += yMovement;

		
		transform.position = currentPosition;
		
	}
	
	void FixedUpdate(){
		//reserved for non rigid/movement operations - this is independent of timescale
	}
	
	void OnTriggerEnter2D( Collider2D other )
	{
		Debug.Log (other.gameObject.name);
		//if powerUp
		if (other.gameObject.name == "fastPowerUp") {

			isFlash = true;
			isMutated = true;

			StartCoroutine(RunningFast ());


			//runningSpeed = 2;
			Object.Destroy (other.gameObject, 0.0F);
			Object.Destroy (GameObject.Find ("fastCube 1"));
			GetComponent<Animator> ().SetBool ("runningFast", true);
		} else if (!(other.gameObject.name == "hole" && _jumping)){
			Application.LoadLevel ("beforeTest");
		}

		
	}

	public IEnumerator RunningFast(){
		runningSpeed = 2.0f;
		yield return new WaitForSeconds(2f); // waits 2 seconds
		runningSpeed = 1.0f;
	}

	
	
}

