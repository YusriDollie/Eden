using UnityEngine;
using System.Collections;

public class WeaponEdge : MonoBehaviour{

    [SerializeField] private float damage;
	GameObject boss;
    // Use this for initialization
    void Start(){
		boss = GameObject.FindGameObjectWithTag ("Boss");
    }
	
    // Update is called once per frame
    void Update(){
	
    }

    void Awake(){
        //com = Weapon.GetComponent<MFPSCombat>();
        //mel = Weapon.GetComponent<MeleeWeapon>();
    }
		

	void OnTriggerEnter(Collider col){
	//	Debug.LogError ("hitting Somethign ");
    	if(col.gameObject.tag == "Player"){
	//		Debug.LogError ("hitting Player"); 
			if (boss.transform.parent.parent.GetComponent<BossAI> ().currentPhase == 0) {
				col.transform.gameObject.GetComponent<Health> ().ApplyDamage (damage);
			}
        }
    }
}
