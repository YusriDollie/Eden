using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// Required when Using UI elements.

public class BarScript : MonoBehaviour{

    [SerializeField] private GameObject player;
    private Health hp;
    //private staminaBar stam;
    private FPSCombat1 fpsCombat;

    private float fillAmount = 0.0f;
    private Image content;

    public bool healthBar;
    public bool staminaBar;

    // Use this for initialization
    void Start(){
        if(healthBar){
            hp = player.gameObject.GetComponent<Health>(); 
        }
        if(staminaBar){

        }
        fpsCombat = player.GetComponent<FPSCombat1>();
        content = this.GetComponent<Image>();
    }
	
    // Update is called once per frame
    void Update(){
		
    }

    void FixedUpdate(){
        HandleBar();
    }

    private void HandleBar(){
        if(healthBar){
            content.fillAmount = Map(hp.currentHealth, 0, hp.getMaxHealth(), 0, 1);
        }
        if(staminaBar){
            content.fillAmount = Map(fpsCombat.currentStamina, 0, fpsCombat.getMaxStamina(), 0, 1);
        }
    }

    private float Map(float value, float inMin, float inMax, float outMin, float outMax){
        float result = (value / inMax) * outMax;
//		Debug.Log ("Health map? " + result);
        return result;
    }
}
