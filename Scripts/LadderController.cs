using UnityEngine;
using System.Collections;

public class LadderController : MonoBehaviour {

	private GameObject playerObject;
	[SerializeField] private float climbSpeed = 1f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate() {
		if (playerObject != null) {
			if (Input.GetKey (KeyCode.W)) {
				playerObject.transform.Translate (new Vector3 (0, 1, 0) * Time.deltaTime * climbSpeed);
			}

			if (Input.GetKey (KeyCode.S)) {
				playerObject.transform.TransformVector (new Vector3 (0, -1, 0) * Time.deltaTime * climbSpeed);
			}
		}
	}

	void OnTriggerEnter (Collider col){
		if (col.tag == "Player") {
			playerObject = col.gameObject;
		}
	}

	void OnTriggerExit (Collider col){
		if (col.tag == "Player") {
			playerObject = null;
		}
	}
}
