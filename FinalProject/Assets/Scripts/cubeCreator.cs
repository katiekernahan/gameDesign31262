using UnityEngine;
using System.Collections;

public class cubeCreator : MonoBehaviour {

	public float minSpawnTime = 0.75f; 
	public float maxSpawnTime = 2f; 

	void Start () {
		Invoke("SpawnCat",minSpawnTime);
	}

	void SpawnCat(){
		//Debug.Log("TODO: Birth a cube at " + Time.timeSinceLevelLoad);
		Invoke("SpawnCat", Random.Range(minSpawnTime, maxSpawnTime));
	}

	
	// Update is called once per frame
	void Update () {
	
	}
}
