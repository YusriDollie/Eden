using UnityEngine;
using System.Collections;

public class WeaponHandler : MonoBehaviour {
	[SerializeField] private GameObject[] weaponArray;
	[SerializeField] private int weaponNum = 0;
	[SerializeField] private GameObject weaponPos;
	[SerializeField] private GameObject curWeapon;
	[SerializeField] bool changeWeapon = true;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate(){
		//Change Weapon
		if (changeWeapon) {
			changeWeapon = false;
			Destroy (curWeapon);
			curWeapon = Instantiate (weaponArray [weaponNum], new Vector3 (weaponPos.transform.position.x,weaponPos.transform.position.y-1.2f, weaponPos.transform.position.z), weaponPos.transform.rotation) as GameObject;
			//curWeapon = Instantiate (weaponArray [weaponNum]);
			curWeapon.transform.parent = weaponPos.transform;
			curWeapon.transform.localPosition = new Vector3 (0, -1.3f, -.15f);
		}

		//Debug
		if (Input.GetKeyDown (KeyCode.O)) {
			changeWeapon = true;
		}

		if (Input.GetKeyDown (KeyCode.I)) {
			weaponNum = 0;
		}

		if (Input.GetKeyDown (KeyCode.K)) {
			weaponNum = 1;
		}
	}
}
