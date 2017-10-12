using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Health : MonoBehaviour{
    //health code
    public const float maxHealth = 100.0f;
    public float currentHealth = maxHealth;
    public bool destroyOnDeath;
    public int deathCount = 0;

    [SerializeField] private GameObject bot;
    private Animator anim;

    public float getMaxHealth(){
        return maxHealth;
    }

    void Start(){
        currentHealth = maxHealth;
        anim = bot.GetComponent<Animator>();
    }

    void Update(){
        //instakill keycode
        if(Input.GetKeyDown(KeyCode.B)){
            currentHealth = 0.0f;
        }
//        if(currentHealth <= 0.0f){
//            if(destroyOnDeath){
//                Death();
//            } else{
//                currentHealth = maxHealth;//was 0 but now we have respawning
//                deathCount++;
//                GetComponent<SpawnManager>().Respawn();          
//            } 
//        }
    }

    public void ApplyDamage(float amount){
        currentHealth -= amount;
        //Debug.LogError(transform.name + " health is" + currentHealth);
        //anim.SetTrigger ("Hit");
        //death code
        if(currentHealth <= 0.0f){
            if(destroyOnDeath){
                Death();
            } else{//reinitialize player and respawn
                currentHealth = maxHealth;//was 0 but now we have respawning
                FPSCombat1 fpsCombat = GetComponent<FPSCombat1>();
                fpsCombat.currentStamina = fpsCombat.getMaxStamina();
                deathCount++;
                GetComponent<SpawnManager>().Respawn();          
            }
        }

        //checks for over health (When picking up health packs)
        if(currentHealth > maxHealth){
            currentHealth = maxHealth;
        }

    }

    void Death(){
        Destroy(this.gameObject); 
    }
        
}