using UnityEngine;
using System.Collections;

public class SentryTurretControl : MonoBehaviour {
	public bool enemySpotted = false;
	public Transform basePlatform;
	public Transform bodyPlatform;
	public Transform aimPoint;
	public float range = 100;
	public float fov = 90;
	public float rof = 100;
	public float aimTime;
	public float fireDelay;
	public static ArrayList targets;

	private GameObject enemy;
	private float angle;

	private float timeLastFired;
	private float timeEnemySpotted;
	private Transform targetAim;
	private Transform startRot;

	// Use this for initialization
	void Start () {
		targets = new ArrayList ();
		angle = fov / 2;
		targetAim = aimPoint;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//If no enemies, look for one
		if (enemy == null) {
			//are there enemies in "Range"
			if (targets.Count > 0) {
				//Step through all players 
				foreach (GameObject tar in targets) {
					if(InRange(tar.transform.position)){
						enemySpotted = true;
						timeEnemySpotted = Time.time;
						enemy = tar;
						break;
					}
				}
			} else {
				enemy = null;
			}
		}

		//check there is an enemy and if enemy is in range													//!!!!!!!!!!!!! DOESN'T CHECK IF DEAD YET !!!!!!!!!!!!!!!!!!!!!
		if (enemySpotted && InRange (enemy.transform.position)) {
			/*targetAim.LookAt (enemy.transform.position);

			Debug.DrawRay (targetAim.position, targetAim.forward * range);*/

			//Lerp aim
			//Quaternion.Slerp (startRot.rotation, targetAim.transform.rotation, Time.deltaTime * aimTime);


			/*Vector3 curAngle = new Vector3 (Mathf.LerpAngle(aimPoint.transform.rotation.x,targetAim.rotation.x,Time.deltaTime * aimSpeed),
				Mathf.LerpAngle(aimPoint.transform.rotation.y,targetAim.rotation.y,Time.deltaTime * aimSpeed),
				Mathf.LerpAngle(aimPoint.transform.rotation.z,targetAim.rotation.z,Time.deltaTime * aimSpeed));

			aimPoint.transform.eulerAngles = curAngle * 117.5f;
			Debug.Log (curAngle + " : " + aimPoint.transform.rotation);*/

			Debug.DrawRay (aimPoint.position, aimPoint.forward * range/2, Color.red);

			//Temp solution
			aimPoint.LookAt (enemy.transform.position);


			basePlatform.transform.rotation = Quaternion.Euler(basePlatform.transform.rotation.x,aimPoint.transform.rotation.y*125,basePlatform.transform.rotation.z);
			bodyPlatform.transform.rotation = Quaternion.Euler(aimPoint.transform.rotation.x*125 - 90,aimPoint.transform.rotation.y*125,bodyPlatform.transform.rotation.z);

			//Debug.Log ("Rotating to intercept");
			//if raycast !Friend Fire


		} else {
			enemy = null;
			enemySpotted = false;
		}
	}

	void OnTriggerEnter (Collider col){
		if (col.tag == "Player") {
			targets.Add (col.gameObject);
		}
	}

	void OnTriggerExit (Collider col){
		if (col.tag == "Player") {
			targets.Remove (col.gameObject);
		}
	}

	private bool InRange(Vector3 target){
		//enemy within cone of view
		if (Vector3.Angle (target - transform.position, transform.forward) < angle) {
			//enemy in range
			if (Vector3.Distance (target, transform.position) < range) {
				return true;
			}
		}
		return false;
	}
}
