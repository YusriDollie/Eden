using UnityEngine;
using System.Collections;

public class LookAtCursor : MonoBehaviour {
	// speed is the rate at which the object will rotate
	public float speed;

	void FixedUpdate () 
	{
		//Mouse Position in the world. It's important to give it some distance from the camera. 
		//If the screen point is calculated right from the exact position of the camera, then it will
		//just return the exact same position as the camera, which is no good.
		Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * 100f);
		float dist = Vector3.Distance (transform.position,mouseWorldPosition);
		float angleX = Mathf.Tan (mouseWorldPosition.x / dist);
		float angleY = Mathf.Tan (mouseWorldPosition.y / dist);

		transform.localRotation = Quaternion.Euler (angleX*25, -angleY*25, 0f);
	}

	float AngleBetweenPoints(Vector2 a, Vector2 b) {
		return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
	}
}
