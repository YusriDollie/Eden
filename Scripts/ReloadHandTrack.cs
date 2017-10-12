using UnityEngine;
using System.Collections;

public class ReloadHandTrack : MonoBehaviour {
	public GameObject hand;
	public float interactDist = 1;
	public float travelDist = 1;
	private float currentTravel = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate(){
		Debug.Log ("Boop");
		if (Vector3.Distance (this.transform.position, hand.transform.position) < interactDist) {
			this.transform.position = new Vector3 (hand.transform.position.x, hand.transform.position.y, hand.transform.position.z);
		} else {
			this.transform.localPosition = new Vector3(0,this.transform.localPosition.y,this.transform.localPosition.z);
		}
		if (this.transform.localPosition.x > 1) {
			//clamp
			this.transform.localPosition = new Vector3(1,this.transform.localPosition.y,this.transform.localPosition.z);
		}else if (this.transform.localPosition.x < 0) {
			//clamp
			this.transform.localPosition = new Vector3(0,this.transform.localPosition.y,this.transform.localPosition.z);
		}
	}
}
