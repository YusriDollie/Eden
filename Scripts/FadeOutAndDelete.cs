using UnityEngine;
using System.Collections;

public class FadeOutAndDelete : MonoBehaviour {
	public GameObject spriteThing;
	public float life = 5f;

	private SpriteRenderer spr;
	// Use this for initialization
	void Start () {
		Destroy (this, life);
		spr = spriteThing.GetComponent<SpriteRenderer> ();

	}
	
	// Update is called once per frame
	void Update () {
		spr.color = Color.Lerp (spr.color, Color.clear, Time.deltaTime * (life/10));
	}
}
