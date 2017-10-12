using UnityEngine;
using System.Collections;

public class EnemySight : MonoBehaviour {


	public float fieldOfViewAngle = 110f;				// Number of degrees, centred on forward, for the enemy see.
	public bool playerInSight;							// Whether or not the player is currently sighted.
	public Transform LastSighting;						//Last seen player position
	private GameObject player;	
	private Transform previousSighting;

	private NavMeshAgent nav;


	void Awake(){
		nav = GetComponent<NavMeshAgent>();
		player = GameObject.FindGameObjectWithTag("Player");


	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	
		if(LastSighting.position != previousSighting.position)
			// ... then update the personal sighting to be the same as the global sighting.
			LastSighting.position = LastSighting.position;

		// Set the previous sighting to the be the sighting from this frame.
		previousSighting.position = LastSighting.position;


	}
}
