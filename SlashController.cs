using UnityEngine;
using System.Collections;

public class SlashController : MonoBehaviour {

	[SerializeField] private float speed;


	// Use this for initialization
	void Awake () {
		Destroy (this, 3f);
	}

	// Update is called once per frame
	void Update () {
		this.transform.Translate (Vector3.forward * speed * Time.deltaTime);
	}
}
