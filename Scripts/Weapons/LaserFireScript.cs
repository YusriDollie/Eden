using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class LaserFireScript : MonoBehaviour {

	LineRenderer line;
	Light light;

	public TextMesh ammoDisplay;

	public bool DMR;
	private bool DMRDeploy = false;

	public float reloadTime = 0;
	private float reloadStart;
	private bool reloading = false;

	public float DMRrof;
	private bool DMRChargeAvail = true;
	private float DMRChargeStart;
	public int DMRMagSize;
	public float SMGrof;
	public int SMGMagSize;
	private float lastFired = -1;
	public AudioClip fire;
	public AudioClip SMGSound;
	public AudioClip DMRSound;
	private AudioSource audioSource;

	public GameObject hitEffect;
	public GameObject[] fireEffect;
	public GameObject scortchMark;

	public Camera leftCam;
	public VignetteAndChromaticAberration leftVnCA;
	public Camera rightCam;
	public VignetteAndChromaticAberration rightVnCA;
	public float effectMax = 60;
	public float effectBuild = 10;
	public float effectVannishRate = 5;
	private bool effectFade = false;
	private float effectFadeState = 0;
	public bool ADS = false;

	public GameObject DMRWings;
	public GameObject[] DMRWingPositions;

	/*public GameObject weapon;
	public GameObject ADSPoint;
	public GameObject weaponStartPos;
	protected Animator animator;*/

	public bool enableEffects = true;

	private int currentMagSize;
	public float currentAmmoCount;

	public Material glow;
	public Color noEnergyColour;
	public Color hasEnergyColour;

	public GameObject energyPowerUp;

	public Material Heat;
	public Color maxHeatColour;
	public Color minHeatColour;
	public float coolDownSpeed;
	private float curHeat = 0;
	private float coolStart;
	private bool startCooling = false;

	// Use this for initialization
	void Start () {
		line = gameObject.GetComponent<LineRenderer> ();
		line.enabled = false;
		audioSource = gameObject.GetComponent<AudioSource> ();
		//Screen.lockCursor = true;
		light = gameObject.GetComponent<Light>();
		light.enabled = false;

		//if (enableEffects) {
		leftVnCA = leftCam.GetComponent<VignetteAndChromaticAberration> ();
		rightVnCA = rightCam.GetComponent<VignetteAndChromaticAberration> ();

		Debug.Log (leftVnCA);
		Debug.Log (rightVnCA);
		//Debug.Log ("Effects");
		//}

		//animator = GetComponent<Animator>();

		rof = SMGrof;
		currentMagSize = SMGMagSize;
		currentAmmoCount = currentMagSize;
		fire = SMGSound;
	}

	float lerpTime = 1f;
	float currentLerpTime;
	float rof;


	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("PropChange")){
			DMR = !DMR;
			if (!DMR) {
				DMRDeploy = false;

				rof = SMGrof;
				//Debug.Log (((currentAmmoCount / SMGMagSize) + 0.5) * DMRMagSize);
				currentAmmoCount = (int)(((currentAmmoCount / DMRMagSize)) * SMGMagSize);
				currentMagSize = SMGMagSize;
				ammoDisplay.text = "" + currentAmmoCount;
				fire = SMGSound;
			} else {
				rof = DMRrof;
				//Debug.Log (((currentAmmoCount / SMGMagSize) + 0.5) * DMRMagSize);
				currentAmmoCount = (int)(((currentAmmoCount / SMGMagSize)) * DMRMagSize);
				currentMagSize = DMRMagSize;
				ammoDisplay.text = "" + currentAmmoCount;
				fire = DMRSound;
			}
			//animator.SetBool("DMR", DMR );
		} 

		//deplay the DMR wings
		if (DMR && !DMRDeploy) {
			DMRDeploy = true;
			StartCoroutine ("DeployWings");
		}

		//Get rid of

		if (Input.GetButtonDown ("PropReload")) {
			//currentAmmoCount = currentMagSize;
			//ammoDisplay.text ="" + currentAmmoCount;
			Debug.Log ("Reload Down");
			reloadStart = Time.time;
			reloading = true;
		}
		if (Input.GetButtonUp ("PropReload")) {
			if ((Time.time > reloadStart + reloadTime)) {
				Debug.Log ("Reload Up");
				currentAmmoCount = currentMagSize;
				ammoDisplay.text ="" + currentAmmoCount;
				//reloadTime = Time.time;
			}
			reloading = false;
		}

		//Do the power up anim of weapon glow
		if (reloading) {

		}



		if (DMR) {
			//DMR firing
			if (Input.GetButtonDown("PropFire")) {
				//check for ammo
				if (currentAmmoCount > 0) {
					//charge shot
					if (DMRChargeAvail) {
						energyPowerUp.SetActive(true);
						//energyPowerUp.GetComponent<Animator> ().SetBool ("ChargeUp", true);
						energyPowerUp.GetComponent<Animator> ().SetTrigger("Charge");
						DMRChargeStart = Time.time;
						DMRChargeAvail = false;
					}
					//energyPowerUp.GetComponent<Animator> ().SetBool ("ChargeUp", false);
				}
			}

			//Fire shot
			if (Input.GetButtonUp("PropFire")){
				DMRChargeAvail = true;
				//check for ammo
				if (currentAmmoCount > 0) {
					if (Time.time > DMRChargeStart + DMRrof) {
						Fire ();
						startCooling = true;
						energyPowerUp.SetActive(false);
						//coolDownSpeed = Time.time;
						//Heat.SetColor ("_EmissionColor", maxHeatColour);
						//Heat.SetFloat ("_EmissionScaleUI", .5f);
					}
				}
				DMRChargeStart = -1;
			}
		} else {
			//SMG firing
			if (Input.GetButton("PropFire")) {
				//Debug.Log ("Pew");
				if (Time.time > lastFired + (rof / 360)) {
					if (currentAmmoCount > 0) {
						Fire ();
					}
				}
			}
		}

		currentLerpTime += Time.deltaTime;
		if (currentLerpTime > lerpTime) {
			currentLerpTime = lerpTime;
		}
		if (enableEffects) {
			float perc = currentLerpTime / lerpTime;
			leftVnCA.chromaticAberration = Mathf.Lerp (effectFadeState, 0, perc);
			rightVnCA.chromaticAberration = Mathf.Lerp (effectFadeState, 0, perc);
		
			if (Input.GetButtonDown ("Fire2")) {
				//Debug.Log ("Beep");
				ADS = true;
				//weapon.transform.position = ADSPoint.transform.position;
				/*Vector3 tempPos = camMain.transform.position;
				tempPos.y -= 0.112f;
				tempPos.z += 0.307f;*/
				//Vector3 tempPos = new Vector3 (0f, -0.112f, 0.307f);
				//weapon.transform.position = tempPos;
			}
			/*
			if (Input.GetButtonUp ("Fire2")) {
				//Debug.Log ("Boop");
				ADS = false;
				weapon.transform.position = weaponStartPos.transform.position;
			}

			if (ADS) {
				weapon.transform.position = ADSPoint.transform.position;
			} else {
				weapon.transform.position = weaponStartPos.transform.position;
			}*/
		}
			

		//Glow effects
		if (currentAmmoCount == 0) {
			glow.color = noEnergyColour;
		} else {
			glow.color = hasEnergyColour;
		}

		//Heat effect
		if (startCooling) {
			
		}
	}

	private void Fire(){
		currentLerpTime = 0f;

		audioSource.PlayOneShot (fire);
		lastFired = Time.time;
		StopCoroutine ("FireLaser");
		StartCoroutine ("FireLaser");

		//eat the bullets
		currentAmmoCount--;
		Debug.Log (currentAmmoCount);
		ammoDisplay.text = "" + currentAmmoCount;

		if (enableEffects) {
			//chromatic effect
			float tempIntensity = leftVnCA.chromaticAberration;
			if (tempIntensity < effectMax) {
				leftVnCA.chromaticAberration += effectBuild;
				rightVnCA.chromaticAberration += effectBuild;
				effectFade = true;
				effectFadeState = leftVnCA.chromaticAberration;
			}
		}

		for (int i = 0; i < fireEffect.Length; i++) {
			fireEffect [i].GetComponent<ParticleSystem> ().Play ();
		}
		//Fire effect
		//GameObject temp = (GameObject)(Instantiate (fireEffect,transform.position, Quaternion.identity));
		//temp.transform.SetParent (gameObject.transform);
		//Destroy (temp, 2F);
	}


	IEnumerator DeployWings(){
		Debug.Log ("DEPLOY ALL THE THINGS");

		float deployDelay = 0.3f;
		//float timeLastDeployed;

		for (int i = 0; i < DMRWingPositions.Length; i++){
			GameObject temp = Instantiate (DMRWings, transform.position, transform.rotation) as GameObject;
			temp.transform.localScale = .5f * transform.localScale;
			temp.GetComponent<Rigidbody> ().AddForce (new Vector3 (Random.Range(-1f,1f),Random.Range(-.1f,1f),Random.Range(-1f,1f)) * 50);
			temp.GetComponent<HeatsinkController> ().target = DMRWingPositions [i];
			temp.GetComponent<HeatsinkController> ().hoverTime += (i * 10 * deployDelay);
			//yield return new WaitForSeconds(deployDelay);
			yield return null;
		}
	}


	IEnumerator FireLaser(){
		line.enabled = true;
		light.enabled = true;

		Ray ray = new Ray (transform.position,- transform.forward);
		//Ray ray = new Ray (Camera.main.transform.position, Camera.main.transform.forward);
		RaycastHit hit;

		line.SetPosition (0, transform.position);

		if (Physics.Raycast (ray, out hit, 100)) {
			line.SetPosition (1, hit.point);
			if (hit.rigidbody) {
				hit.rigidbody.AddForceAtPosition (transform.forward * -500, hit.point);
				GameObject temp = GameObject.Instantiate (hitEffect, hit.point, Quaternion.identity) as GameObject;
				//temp.gameObject.transform.SetParent(hit.transform);
				temp.transform.parent = hit.transform.gameObject.transform;

				Destroy (temp, 2f);

				GameObject scortch = GameObject.Instantiate (scortchMark, hit.point, Quaternion.identity) as GameObject;
				//temp.gameObject.transform.SetParent(hit.transform);
				scortch.transform.parent = hit.transform.gameObject.transform;
				scortch.transform.rotation = Quaternion.FromToRotation (Vector3.forward, hit.normal);

				//Temp
				if (hit.rigidbody.tag == "Enemy") {
					Destroy (hit.rigidbody.gameObject, 2f);
				}
			}
		} else {
			line.SetPosition (1, ray.GetPoint (100));
		}

			//yield return null;
		//}

		yield return new WaitForSeconds (.05f);

		line.enabled = false;
		light.enabled = false;
	}
}
