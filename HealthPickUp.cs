using UnityEngine;
using System.Collections;

public class HealthPickUp : MonoBehaviour{
    public bool stamina = false;

    void OnTriggerEnter(Collider col){
        Debug.Log("KNok");
        if(col.tag == "Player"){
            if(!stamina){
                Health health = col.GetComponent<Health>();
                if(health.currentHealth < 100.0f){//prevent extrea health from pickups
					if (this.gameObject.tag == "L") {
						health.ApplyDamage(-100.0f);
						Destroy(this.gameObject);

					} else if (this.gameObject.tag == "M") {
						health.ApplyDamage(-30.0f);
						Destroy(this.gameObject);

					} else {

						health.ApplyDamage(-10.0f);
						Destroy(this.gameObject);
					}
                    
                }
            } else{
                FPSCombat1 fpsCombat = col.GetComponent<FPSCombat1>();
                if(fpsCombat.currentStamina < 100.0f){//prevent extrea health from pickups
                    fpsCombat.ChangeStamina(20.0f);
                    Destroy(this.gameObject);
                }
            }
        }
    }
}
