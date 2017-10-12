using UnityEngine;
using System.Collections;

public class ObjectPositionFollow : MonoBehaviour {
	public GameObject trackingObject;

	public bool x = true;
	public bool y = false;
	public bool z = true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector3 temp = new Vector3 (0, 0, 0);

		if (x) {
			temp = new Vector3 (trackingObject.transform.position.x, temp.y, temp.z);
		}
		if (y) {
			temp = new Vector3 (temp.x, trackingObject.transform.position.y, temp.z);
		}
		if (z) {
			temp = new Vector3 (temp.x, temp.y, trackingObject.transform.position.z);
		}

		this.transform.position = temp;
	}
}
