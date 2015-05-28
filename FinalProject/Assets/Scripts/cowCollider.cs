using UnityEngine;
using System.Collections;

public class cowCollider : MonoBehaviour {
	
	//LEVEL ON
	static public int level = 1;
	
	//SUPERPOWERS
	private float runningSpeed = 1.0f;
	private bool _flying = false;
	private bool _isHulk = false;
	
	private float isometricAngle =  Mathf.Tan ((26.5f * Mathf.PI)/180);
	
	//Sorting Order
	public int directionID = 6;
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
	private bool _slidingUp = false;
	private bool _slidingDown = false;
	private float _distanceFromOriginSlide = 0.0f;
	private int bounceNo = 0;
	
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

		if (directionID == 1) 
		{
			_slidingUp = true;
		}
		else if (directionID == 2)
		{
			_slidingRight = true;
		}
		else if (directionID == 3)
		{
			_slidingDown = true;
		}
		else if (directionID == 4)
		{
			_slidingLeft = false;
		}

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
			float xMovement = -1 * runningSpeed / 20;
			//float yMovement = isometricAngle * 1.003f * runningSpeed / 10;
			xMovement += move * 0.5f / 2;
			//yMovement -= move * 0.5f / 2;
			
			currentPosition.x += xMovement;
			//currentPosition.y += yMovement;
			
			transform.position = currentPosition;
			
		} 
		else if (_slidingRight) 
		{
			float xMovement = -1 * runningSpeed / 20;
			//float yMovement = isometricAngle * 1.003f * runningSpeed / 10;
			xMovement += move * 0.5f / 2;
			//yMovement -= move * 0.5f / 2;
			
			currentPosition.x -= xMovement;
			//currentPosition.y -= yMovement;
			
			transform.position = currentPosition;
		}
		else if (_slidingUp) 
		{
			//float xMovement = -1 * runningSpeed / 10;
			float yMovement = isometricAngle * 1.003f * runningSpeed / 20;
			//xMovement += move * 0.5f / 2;
			yMovement -= move * 0.5f / 2;
			
			//currentPosition.x += xMovement;
			currentPosition.y += yMovement;
			
			transform.position = currentPosition;
			
		} 
		else if (_slidingDown) 
		{
			//float xMovement = -1 * runningSpeed / 10;
			float yMovement = isometricAngle * 1.003f * runningSpeed / 20;
			//xMovement += move * 0.5f / 2;
			yMovement -= move * 0.5f / 2;
			
			//currentPosition.x -= xMovement;
			currentPosition.y -= yMovement;
			
			transform.position = currentPosition;
		}
	}
	
	void FixedUpdate(){
		//reserved for non rigid/movement operations - this is independent of timescale
	}

	/* Movement paths are up - right - up - right - up - etc
	 * and
	 * down - left - down - left - down - etc
     */
	public void changeMovement()
	{
		if (_slidingUp) //from up to right
		{

			_slidingLeft = false;
			_slidingRight = true;  //now slide right
			_slidingUp = false;
			_slidingDown = false;
		} 
		else if (_slidingRight) 
		{
			_slidingLeft = false;
			_slidingRight = false;  
			_slidingUp = true;     //now slide up
			_slidingDown = false;
		}
		else if (_slidingDown) 
		{
			_slidingLeft = true; //now slide left
			_slidingRight = false;  
			_slidingUp = false;     
			_slidingDown = false;
		}
		else if (_slidingLeft)
		{
			_slidingLeft = false; 
			_slidingRight = false;  
			_slidingUp = false;     
			_slidingDown = true; //now slide left
		}
		bounceNo++;
	}

	void stop()
	{
		_slidingLeft = false;
		_slidingRight = false;  
		_slidingUp = false;     
		_slidingDown = false;
	}

	void OnTriggerEnter2D( Collider2D other )
	{
		if (bounceNo > 10) 
		{
			stop ();
		}
		else
		{
			changeMovement ();
		}

	}
	
}