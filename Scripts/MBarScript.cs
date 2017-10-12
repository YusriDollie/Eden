using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// Required when Using UI elements.

public class MBarScript : MonoBehaviour{

    public GameObject player;
    private MHealth hp;
    //private staminaBar stam;
    private MFPSCombat mfpsCombat;

    private float fillAmount = 0.0f;
    private Image content;

    public bool healthBar;
    public bool staminaBar;

    public float health;
    public float stamina;

    // Use this for initialization
    void Start(){
        content = this.GetComponent<Image>();
    }

    public void Init(){
        if(healthBar){
            hp = player.gameObject.GetComponent<MHealth>(); 
            health = hp.currentHealth;
        }
        if(staminaBar){
            mfpsCombat = player.GetComponent<MFPSCombat>();
            stamina = mfpsCombat.currentStamina;
        }
    }
	
    // Update is called once per frame
    void Update(){
		
    }

    void FixedUpdate(){
//        HandleBar();
    }

    public void HandleBar(){
        if(healthBar){
            content.fillAmount = Map(hp.currentHealth, 0, hp.getMaxHealth(), 0, 1);
        }
        if(staminaBar){
            content.fillAmount = Map(mfpsCombat.currentStamina, 0, mfpsCombat.getMaxStamina(), 0, 1);
        }
    }

    private float Map(float value, float inMin, float inMax, float outMin, float outMax){
        float result = (value / inMax) * outMax;
//		Debug.Log ("Health map? " + result);
        return result;
    }
}
