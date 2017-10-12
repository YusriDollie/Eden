using UnityEngine;
using System.Collections;

public class SceneLoaderController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Y)) {
			Application.LoadLevel(Application.loadedLevel);
		}
	}
}
