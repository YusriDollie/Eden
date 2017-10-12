using UnityEngine;
using System.Collections;

public class DebugPlayerMover : MonoBehaviour {
	[SerializeField] private GameObject playerObject;
	[SerializeField] private Transform[] positions;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			playerObject.transform.position = positions [0].position;
			playerObject.transform.rotation = positions [0].rotation;
		}
		if (Input.GetKeyDown (KeyCode.Alpha2)) {
			playerObject.transform.position = positions [1].position;
			playerObject.transform.rotation = positions [1].rotation;
		}
		if (Input.GetKeyDown (KeyCode.Alpha3)) {
			playerObject.transform.position = positions [2].position;
			playerObject.transform.rotation = positions [2].rotation;
		}
		if (Input.GetKeyDown (KeyCode.Alpha4)) {
			playerObject.transform.position = positions [3].position;
			playerObject.transform.rotation = positions [3].rotation;
		}
		if (Input.GetKeyDown (KeyCode.Alpha5)) {
			playerObject.transform.position = positions [4].position;
			playerObject.transform.rotation = positions [4].rotation;
		}
	}
}
