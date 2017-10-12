using UnityEngine;
using System.Collections;

public class RifleHUDBalance : MonoBehaviour {
	public GameObject mainBody;
	public GameObject horizon;
	public GameObject compas;
	public GameObject floor;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate(){
		//compas.transform.rotation = Quaternion.Euler(new Vector3(floor.transform.rotation.x + 90,floor.transform.rotation.y,floor.transform.rotation.z));
		compas.transform.rotation = floor.transform.rotation;
	}
}
