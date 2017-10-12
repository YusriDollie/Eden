using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class MHealth : NetworkBehaviour{
    public const float maxHealth = 100.0f;
    [SyncVar(hook = "OnChangeHealth")] public float currentHealth = maxHealth;
    //clients will know if the syncvar is changed to call this method
    //when ever the value changes on the server it will inform the clients
    //it passes the updated value
    public bool destroyOnDeath;
    public int deathCount = 0;
    public NetworkStartPosition[] spawnPoints;
    private GameObject healthbar;


    public float getMaxHealth(){
        return maxHealth;
    }

    void Start(){
        //may shift spawn code to a networkspawnManager
        if(isLocalPlayer){
            spawnPoints = FindObjectsOfType<NetworkStartPosition>();
            healthbar = GameObject.FindGameObjectWithTag("HealthBar");
            healthbar.GetComponent<MBarScript>().player = this.gameObject;
            healthbar.GetComponent<MBarScript>().Init();
        }
    }

    void Update(){
        //instakill code
        if(isLocalPlayer){
            if(Input.GetKeyDown(KeyCode.B)){
                currentHealth = 0.0f;
            }
        }
    }

    public void ApplyDamage(float amount){
        if(!isServer){//if a client do nothing (jumps out of the method)
            Debug.LogError(gameObject.name + " got hit");
            return;
        }
        currentHealth -= amount;
        Debug.LogError(transform.name + " health is" + currentHealth);
        if(currentHealth <= 0.0f){
            if(destroyOnDeath){
                Death();
            } else{
                //reinitialize player and respawn
                currentHealth = maxHealth;//was 0 but now we have respawning
                MFPSCombat mfpsCombat = GetComponent<MFPSCombat>();
                mfpsCombat.currentStamina = mfpsCombat.getMaxStamina();
                deathCount++;
                RpcRespawn();              
            }
        }
    }

    void Death(){
        GameManager.DeRegisterPlayer(transform.GetComponent<NetworkPlayerController>().playerID);
        Destroy(this.gameObject); 
    }

    //syncs up the healthbar ui with the sync var health by passing it in
    void OnChangeHealth(float health){
        //updates health bar
        currentHealth = health;
        if(isLocalPlayer){
            healthbar.GetComponent<MBarScript>().HandleBar();     
        }
    }

    [ClientRpc]//runs the function on all clients using data from the server
    void RpcRespawn(){
        if(isLocalPlayer){
            Vector3 spawnPoint = Vector3.zero;
            if(spawnPoints != null && spawnPoints.Length > 0){
                spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
            }
            transform.position = spawnPoint;
            //transform.position = Vector3.zero;
        }
    }


}