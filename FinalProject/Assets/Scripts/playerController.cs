using UnityEngine;
using System.Collections;

public class playerController : MonoBehaviour {

	//LEVEL ON
	static public int level = 1;

	//Paused
	bool _paused;

	//Data Hook
	int retryCount = 0;
	int currentlevel = 1;

	//SUPERPOWERS
	private float runningSpeed = 1.0f;
	private bool _flying = false;
	private bool _isHulk = false;

	private float isometricAngle =  Mathf.Tan ((26.5f * Mathf.PI)/180);

	//Sorting Order
	public int sortingOrder = 6;
	private SpriteRenderer sprite;

	//Movement
	private float fractionMovement = 1.0f;
	private Rigidbody2D _myRigidBody;
	private bool _jumping = false;
	private float _distanceFromGround = 0.0f;
	private Vector3 currentPosition;
	private bool _jumpingUp = false;
	private bool _slidingLeft = false;
	private bool _slidingRight = false;
	private float _distanceFromOriginSlide = 0.0f;

	//Shadow
	private GameObject shadow;
	private Vector3 shadowPosition;
	private Vector3 shadowScale;

	//Player Collider
	private GameObject playerCollider;
	private Vector3 playerColliderPosition;

	//Camera
	private GameObject camera;
	private Vector3 cameraPosition;

	//Mechanim
	private Animator animator;

	public GameObject explosion;


	
	void Start () {
		Debug.Log ("Level is: " + Application.loadedLevelName);
		string levelName = Application.loadedLevelName;
		//ASCII value
		int levelIs = (levelName [levelName.Length - 1]) - 48;
		Debug.Log ("Level is: " + levelIs);
		level = levelIs;

		explosion = GameObject.Find ("explosion");

		Time.timeScale = 1.0F;

		_myRigidBody = GetComponent<Rigidbody2D>();
		shadow = GameObject.Find ("shadow");
		playerCollider = GameObject.Find ("playerCollider");
		sprite = GetComponent<SpriteRenderer>();
		camera = GameObject.Find ("Main Camera");

		//run level start
		//Time.timeScale = 0.0f;
		//pause and then re do
		//Time.timeScale = 1.0f;
		StartCoroutine (startOfLevel ()); 

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
			if(_distanceFromGround < 1)
				GetComponent<Animator> ().SetBool ("jumping", false);
			if (_distanceFromGround < 0){
				_jumping = false;
			}
		}
		shadow.transform.position = shadowPosition;
		shadow.transform.localScale = shadowScale;
		playerCollider.transform.position = playerColliderPosition;
	}

	
	// Update is called once per frame
	void Update () {

		if (!_paused) {

			currentPosition = transform.position;

			float move = 0;


			if (Input.GetKeyDown ("left") && !_jumping) {
				//move = -5;
				//slideLeft();
				if (sortingOrder != 7 && !_slidingRight && !_slidingLeft) {
					_slidingLeft = true;
					sortingOrder++;
					sprite.sortingOrder--;
				}
			}

			if (Input.GetKeyDown ("right") && !_jumping) {

				if (sortingOrder != 0 && !_slidingLeft && !_slidingRight) {
					_slidingRight = true;
					sprite.sortingOrder++;
					sortingOrder--;
				}
				//slideRight();
			}

			
			if (Input.GetKeyDown ("space")) {
				Debug.Log ("Space hit");
				if (!_jumping) {

					_jumping = true;
					_jumpingUp = true;
					GetComponent<Animator> ().SetBool ("jumping", true);
				}
			}

			if (_jumping) {
				jump ();
			}


			float xMovement = 1 * runningSpeed / 10;
			float yMovement = isometricAngle * 1.003f * runningSpeed / 10;
			
			if (_slidingLeft) {
				if (_distanceFromOriginSlide < 4.2f) { //fraction to move in between lines
					move -= 0.5f;
					_distanceFromOriginSlide += 0.5f; 
				} else {
					_slidingLeft = false;
					_distanceFromOriginSlide = 0;
				}
			} else if (_slidingRight) {
				if (_distanceFromOriginSlide < 4.2f) {
					move += 0.5f;
					_distanceFromOriginSlide += 0.5f;
				} else {
					_slidingRight = false;
					_distanceFromOriginSlide = 0;
				}
			} 

			xMovement += move * 0.5f / 2;
			yMovement -= move * 0.5f / 2;
				
				
				
			currentPosition.x += xMovement * Time.timeScale;
			currentPosition.y += yMovement * Time.timeScale;

			
			transform.position = currentPosition;
		}
	}
	
	void FixedUpdate(){
		//reserved for non rigid/movement operations - this is independent of timescale
	}

	//Using Stay instead of enter to detect jumping
	void OnTriggerStay2D( Collider2D other )
	{
		Debug.Log (other.gameObject.name);
		//if powerUp

		if (other.gameObject.name == "endLevelCollider") {
			Debug.Log ("Load Next Level");
			StartCoroutine (LoadNextLevel ());
		}
		else if (other.transform.parent.gameObject.name.StartsWith ("power")) {
			switch (other.gameObject.name) {
			case "fastPowerUp":
				StartCoroutine (RunningFast ());
				break;
			case "hulkPowerUp":
				StartCoroutine (HulkSmash ()); 
				break;
			case "superManPowerUp":
				StartCoroutine (Flying ()); 
				break;
			default:
				Debug.Log ("No powerup with this name");
				break;
			}
			Object.Destroy (other.transform.parent.gameObject, 0.0F);

		}
		else if (!(other.gameObject.name == "hole" && (_jumping || _flying))){
			//reload the level because there was a crashe

			//Nested If statement because I can
			if(!_isHulk && !_flying)
			{
				runningSpeed = 0.0f;
				Time.timeScale = 0.0f;



				transform.gameObject.AddComponent<RetryController>();
			}
			else if(_isHulk){
				Debug.Log ("Explode Item!");
				Object.Destroy (other.gameObject, 0.0F);
				Object.Destroy (other.transform.parent.gameObject, 0.0F);
				//GameObject explosion = new GameObject("Explosion");
				//explosion.transform.position = transform.position;

				GameObject expl = Instantiate(explosion, transform.position, Quaternion.identity) as GameObject; // destroy the grenade
				Destroy(expl, 1); // delete the explosion after 3 seconds
			}
			else{
				//isflying
			}
				//Application.LoadLevel ("beforeTest");
		}
		Debug.Log ("Must be jumping: " + _jumping);
		
	}

	public IEnumerator RunningFast(){
		//should be using an animator object instead of always referencing
		GetComponent<Animator> ().SetBool ("fast", true);	
		runningSpeed = 2.0f;
		yield return new WaitForSeconds(3f); // waits 2 seconds
		yield return new WaitForSeconds(0.2f);
		GetComponent<Animator> ().SetBool ("fast", true);
		yield return new WaitForSeconds(0.2f);
		GetComponent<Animator> ().SetBool ("fast", false);
		yield return new WaitForSeconds(0.2f);
		GetComponent<Animator> ().SetBool ("fast", true);
		yield return new WaitForSeconds(0.2f);
		runningSpeed = 1.0f;
		GetComponent<Animator> ().SetBool ("fast", false);
	}

	public IEnumerator HulkSmash(){
		_isHulk = true;
		Debug.Log ("Hulk smash");
		GetComponent<Animator> ().SetBool ("hulk", true);
		yield return new WaitForSeconds(3f); // waits 2 seconds
		GetComponent<Animator> ().SetBool ("hulk", false);
		yield return new WaitForSeconds(0.2f);
		GetComponent<Animator> ().SetBool ("hulk", true);
		yield return new WaitForSeconds(0.2f);
		GetComponent<Animator> ().SetBool ("hulk", false);
		yield return new WaitForSeconds(0.2f);
		GetComponent<Animator> ().SetBool ("hulk", true);
		yield return new WaitForSeconds(0.2f);
		GetComponent<Animator> ().SetBool ("hulk", false);
		_isHulk = false;
	}

	public IEnumerator Flying(){
		_flying = true;
		GetComponent<Animator> ().SetBool ("flying", true);
		yield return new WaitForSeconds(5f); // waits 2 seconds
		yield return new WaitForSeconds(0.2f);
		GetComponent<Animator> ().SetBool ("flying", true);
		yield return new WaitForSeconds(0.2f);
		GetComponent<Animator> ().SetBool ("flying", false);
		yield return new WaitForSeconds(0.2f);
		GetComponent<Animator> ().SetBool ("flying", true);
		yield return new WaitForSeconds(0.2f);
		GetComponent<Animator> ().SetBool ("flying", false);
		_flying = false;
	}

	public IEnumerator LoadNextLevel(){
		transform.DetachChildren ();
		yield return new WaitForSeconds(3f); // waits 2 seconds
		Debug.Log ("Level was: " + level);
		Application.LoadLevel ("Level" + (level + 1));
	}

	public IEnumerator startOfLevel(){
		Time prevTime;
		//But time scale is 0 so 3 seconds will never happen!

		_paused = true;
		yield return new WaitForSeconds (1.5f);
		_paused = false;

	}
	
	
}

