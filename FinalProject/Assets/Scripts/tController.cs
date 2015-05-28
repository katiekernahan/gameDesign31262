using UnityEngine;
using System.Collections;

public class tController : MonoBehaviour {
	
	//LEVEL ON
	static public int level = 1;
	
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
	private bool _slidingLeft = true;
	private bool _slidingRight = false;
	private float _distanceFromOriginSlide = 0.0f;
	private int dirchange = 0;
	
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

		Time.timeScale = 1.0F;
		
		shadow = GameObject.Find ("shadow");
		playerCollider = GameObject.Find ("playerCollider");
		sprite = GetComponent<SpriteRenderer>();
	}

	// Update is called once per frame
	void Update () 
	{
		currentPosition = transform.position;
		float move = 0;
		if (_slidingLeft) 
		{
			float xMovement = -1 * runningSpeed / 10;
			float yMovement = isometricAngle * 1.003f * runningSpeed / 10;
			xMovement += move * 0.5f / 2;
			yMovement -= move * 0.5f / 2;
			
			currentPosition.x += xMovement;
			currentPosition.y += yMovement;
			
			transform.position = currentPosition;

		} 
		else if (_slidingRight) 
		{
			float xMovement = -1 * runningSpeed / 10;
			float yMovement = isometricAngle * 1.003f * runningSpeed / 10;
			xMovement += move * 0.5f / 2;
			yMovement -= move * 0.5f / 2;
			
			currentPosition.x -= xMovement;
			currentPosition.y -= yMovement;
			
			transform.position = currentPosition;
		}
	}
	
	void FixedUpdate(){
		//reserved for non rigid/movement operations - this is independent of timescale
	}

	public void reverseMovement()
	{
		if (_slidingLeft) 
		{
			_slidingLeft = false;
			_slidingRight = true;
		} 
		else if (_slidingRight) 
		{
			_slidingLeft = true;
			_slidingRight = false;
		}
	}

	public IEnumerator LoadNextLevel(){
		transform.DetachChildren ();
		yield return new WaitForSeconds(3f); // waits 2 seconds
		Debug.Log ("Level was: " + level);
		Application.LoadLevel ("Level" + (level + 1));
	}
	
	void OnTriggerEnter2D( Collider2D other )
	{
		reverseMovement ();
		Debug.Log (other.gameObject.name);		
		if (other.gameObject.name == "lvl3_roadside_hedge") {
			Debug.Log ("hit");
		}
	}

}