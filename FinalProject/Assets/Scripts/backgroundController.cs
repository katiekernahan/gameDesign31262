using UnityEngine;
using System.Collections;

public class backgroundController : MonoBehaviour {

	private Transform cameraTransform; //better than finding it every update() call
	private float spriteWidth; //cache sprites width since you know it won’t change
	private float spriteHeight;
	private SpriteRenderer spriteRenderer;


	// Use this for initialization
	void Start () {
		cameraTransform = Camera.main.transform;
		spriteRenderer = GetComponent<SpriteRenderer>(); 
		spriteWidth = spriteRenderer.sprite.bounds.size.x;
		spriteHeight = spriteRenderer.sprite.bounds.size.y;
	}
	
	// Update is called once per framegsd
	void Update () {
		if( (transform.position.x + spriteWidth) < cameraTransform.position.x) {
			Vector3 newPos = transform.position;
			newPos.x += 1.695f * spriteWidth*Time.timeScale; 
			newPos.y += 1.305f * spriteHeight*Time.timeScale;
			transform.position = newPos;
			}

	}
	void OnTriggerEnter2D( Collider2D other )
	{
		
		Debug.Log ("Hit " + other.gameObject);
		Time.timeScale = 0.5f;
		
	}
}
