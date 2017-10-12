using UnityEngine;
using System.Collections;

public class HeatsinkController : MonoBehaviour {
	public GameObject mainBody;
	public GameObject leftWing;
	private Quaternion leftStart;
	public GameObject rightWing;
	private Quaternion rightStart;

	public GameObject target;

	public float hoverTime;
	public float travelSpeed;
	//public float wingOpenTime;

	private float awakeTime;
	private float startTime;
	private Vector3 startPos;
	private Quaternion startRot;

	private bool startMove = true;
	private bool rotation = false;


	public Material heatFins;
	public Color32 CoolColor;
	public Color32 HotColor;
	private float temperature = 0;
	public float coolTime = 1f;
	private float coolStart;




	void Awake(){
		awakeTime = Time.time;
		startTime = awakeTime + hoverTime;
		//HotColor = HotColor * Mathf.LinearToGammaSpace (temperature);
	}

	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("PropChange")){
			Destroy (this.gameObject);
		}

		/*if ((Input.GetButton ("Fire1"))&&rotation) {
			StopCoroutine (HeatCooldown ());
			StartCoroutine (HeatCooldown ());
		}*/

		if (Time.time > startTime) {
			if (startMove) {
				startPos = transform.position;
				startRot = transform.rotation;
				startMove = false;
			}

			float distCovered = (Time.time - startTime) * travelSpeed;
			//float fracJourney = distCovered / journeyLength;
			transform.position = Vector3.Lerp (startPos, target.transform.position, distCovered);
			transform.rotation = Quaternion.Slerp (startRot, target.transform.rotation, distCovered);

			if (transform.position == target.transform.position) {
				transform.SetParent (target.transform);
				//StartCoroutine ("DeployWings");
				rotation = true;
				leftStart = leftWing.transform.rotation;
				rightStart = rightWing.transform.rotation;
			}

			if (rotation) {
				float delta = Time.deltaTime;
				float speed = 400;
				rightWing.transform.localRotation = Quaternion.RotateTowards (rightWing.transform.localRotation, Quaternion.Euler(new Vector3 (0, 0, 120)), delta * speed);
				leftWing.transform.localRotation = Quaternion.RotateTowards (leftWing.transform.localRotation, Quaternion.Euler(new Vector3 (0, 0, -120)), delta * speed);
			}
		} else {

		}
	}

	

	IEnumerator HeatCooldown(){
		temperature = 1;
		Color curColor = HotColor;

		heatFins.SetFloat ("_EmissionScaleUI", temperature);
		heatFins.SetColor ("_EmissionColor", curColor);
		//heatFins.color = temperature;

		coolStart = Time.time;

		while (Time.time < coolStart + coolTime) {
			float progressTime = (coolStart + coolTime) / Time.time;

			Color.Lerp (curColor, CoolColor, Time.deltaTime * progressTime);
			temperature = Mathf.Lerp(temperature, 0, Time.deltaTime * progressTime);

			heatFins.SetFloat ("_EmissionScaleUI", temperature);
			heatFins.SetColor ("_EmissionColor", curColor);
			//finalColor = HotColor * Mathf.LinearToGammaSpace (temperature);
		}

		yield return 0;

	}
		
}
