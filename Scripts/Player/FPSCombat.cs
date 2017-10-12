using UnityEngine;
using System.Collections;

public class FPSCombat : MonoBehaviour {

	[SerializeField] private GameObject weapon;
	private MeleeWeapon weaponScript;

	[SerializeField] private GameObject arms;
	private Animator anim;
	private Animation anims;

	private float lastAttack = 0;
	private float latestNextAttack;
	public int comboPos = 0;
	private Vector2 inputs;

	// Use this for initialization
	void Awake () {
		weaponScript = weapon.GetComponent<MeleeWeapon> ();
		arms = weaponScript.arms;
		anim = arms.GetComponent<Animator> ();
		anims = arms.GetComponent<Animation> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Fire1")) {
			float attackTime = lastAttack + weaponScript.comboDelay [comboPos];
			if (Time.time > attackTime) {	//chec if in time to continue with combo
				comboPos++;

				if (comboPos > weaponScript.combo.Length - 1) {
					comboPos = 0;
				}

				if (Time.time > attackTime + weaponScript.ROFDeadzone) {
					comboPos = 0;
				}
				//Debug.Log (Time.time + "\tLastAttack: " + lastAttack + "\tComboPos: " + comboPos);
				switch (comboPos) {
					case 0:
						anim.SetTrigger ("AttackLight0");
						break;
					case 1:
						anim.SetTrigger ("AttackLight1");
						break;
					case 2:
						anim.SetTrigger ("AttackLight2");
						break;
					case 3:
						anim.SetTrigger ("AttackLight3");
						break;
					default:
						break;
				}
				lastAttack = Time.time;
			}
		}
	}

	void FixedUpdate(){
		anim.SetBool ("Midair", !this.gameObject.GetComponentInParent<CharacterController>().isGrounded);
		/*if (Physics.Raycast(this.transform.position, -this.transform.parent.transform.up , 1.5f)){

		}else{
			anim.SetBool ("Midair", false);
		}*/

		InputCheck ();

		if (inputs.magnitude > 0) { //makes sure that player cant "run on spot"
			if (Input.GetKey (KeyCode.LeftShift)) {
				anim.SetBool ("Run", true);
			} else {
				anim.SetBool ("Run", false);
			} 
		}

		if (Input.GetKeyDown (KeyCode.E)) {
			anim.SetBool ("Interact", true);
		} else if (Input.GetKeyUp (KeyCode.E)) {
			anim.SetBool ("Interact", false);
		}
	}

	void InputCheck(){
		float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis("Vertical");

		inputs = new Vector2 (horizontal, vertical);
	}
}
