using UnityEngine;
using System.Collections.Generic;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.Networking;

public class MFPSCombat : NetworkBehaviour{

    [SerializeField]public GameObject weapon;

    public GameObject arms;
    private Animator anim;
    private Animation anims;

    private float lastAttack = 0;
    private float latestNextAttack;
    public int comboPos = 0;
    private Vector2 inputs;

    private Camera cam;
    private MeleeWeapon mel;

    private bool gotWeapon = false;

    //stamina code
    //NB May need a syncvar with a hook for OnStaminaChange
    private const float maxStamina = 100.0f;
    [SyncVar(hook = "OnChangeStamina")] public float currentStamina = maxStamina;
    public float staminRegenRate = 3.0f;
    private GameObject staminabar;

    private CharacterController chController;
    private FirstPersonController fpController;
    private MWeaponHandler weaponHandler;

    //sound effects
    [SerializeField] private AudioClip[] meleeSounds;
    private AudioSource audioSource;

    public Dictionary <int, NetworkPlayerController> players = new Dictionary<int, NetworkPlayerController>();

    //layer mask to control what the ray hits i.e. other players
    [SerializeField] private LayerMask mask;

    //network animation code
    public NetworkAnimator netAnim;
    // Use this for initialization

    void Start(){
        if(isLocalPlayer){
            staminabar = GameObject.FindGameObjectWithTag("StaminaBar");
            staminabar.GetComponent<MBarScript>().player = this.gameObject;
            staminabar.GetComponent<MBarScript>().Init();  
        }
    }

    void Awake(){
        weaponHandler = GetComponent<MWeaponHandler>();
        InitWeapon(weaponHandler.curWeapon);
        cam = Camera.main;
        chController = GetComponent<CharacterController>();
        fpController = GetComponent<FirstPersonController>();
        audioSource = GetComponent<AudioSource>();
    }

    //sets and spawn players weapon and attaches it to the weapon camera
    public void InitWeapon(GameObject w){
        weapon = w;
        arms = weapon.transform.GetChild(0).gameObject;
        anim = arms.GetComponent<Animator>();
        anims = arms.GetComponent<Animation>();
        mel = weapon.GetComponent<MeleeWeapon>();
    }

    public float getMaxStamina(){
        return maxStamina;
    }

    // Update is called once per frame
    void Update(){
        //use more stamina when running
        if(chController.velocity.magnitude > 0 && Input.GetKey(KeyCode.LeftShift)){
            CmdChangeStamina(Time.deltaTime * -10.0f);
        }

        //stamina regeneration all the time including if we aren't running
        if(!Input.GetKey(KeyCode.LeftShift) && (currentStamina >= 0)){
            if(isLocalPlayer){
                CmdChangeStamina(Time.deltaTime * staminRegenRate);

            }

        }
//        //if stamina is fully regenerated
        if(currentStamina >= maxStamina){
            currentStamina = maxStamina;
        }
//
//        //if stamina is empty
        if(currentStamina <= 0){
            currentStamina = 0;
        }  

        if(Input.GetButtonDown("Fire1")){
            if(!isLocalPlayer){
                return;
            } else{
                if(currentStamina < 20.0f){
                    return;
                } else{
                    float attackTime = lastAttack + mel.comboDelay[comboPos];
                    if(Time.time > attackTime){ //check if in time to continue with combo
                        comboPos++;

                        if(comboPos > mel.combo.Length - 1){
                            comboPos = 0;
                        }

                        if(Time.time > attackTime + mel.ROFDeadzone){
                            comboPos = 0;
                        }
                            
                        RaycastHit hit;

                        if(Physics.Raycast(this.transform.position, this.transform.forward.normalized, out hit, mel.comboDist[comboPos], mask)){//using -1 to hit on all layers [temporary]
                            Debug.LogError("Fucker " + hit.transform.tag);
                            if((hit.transform.tag == "Enemy") || (hit.transform.tag == "Player") || (hit.transform.tag == "BreakableObject") || (hit.transform.tag == "Boss")){
//                                Debug.LogError("Found something to hit! that something is " + hit.transform.name);
                                if(hit.transform.tag == "Player"){
                                    if(gameObject.GetComponent<NetworkPlayerController>().playerID != hit.transform.GetComponent<NetworkPlayerController>().playerID){
                                        //play punch sound
                                        audioSource.PlayOneShot(meleeSounds[0]);
                                        //run atack command passing in hitter and hit netid's and the amount of damage depending on the combo
                                        CmdAttack(mel.comboDamage[comboPos], hit.transform.gameObject);
                                    }   
                                } else{
                                    //play punch sound
                                    audioSource.PlayOneShot(meleeSounds[0]);
                                    //run atack command passing in hitter and hit netid's and the amount of damage depending on the combo
                                    CmdAttack(mel.comboDamage[comboPos], hit.transform.gameObject); 
                                }
                            } else{
                                if(hit.transform.name != null){
                                    Debug.LogError("hit " + hit.transform.name);
                                } else{
                                    Debug.LogError("hit nothing!");
                                }
                            }
                        } else{
                            //play punch empty air sound
                            audioSource.PlayOneShot(meleeSounds[1]);  
                        }

                        //Debug.Log (Time.time + "\tLastAttack: " + lastAttack + "\tComboPos: " + comboPos);
                        switch(comboPos){
                        case 0:
                            anim.SetTrigger("AttackLight0");
                            netAnim.SetTrigger("Attack1");
                            //RaycastHit rayhit = Physics.Raycast
                            break;
                        case 1:
                            anim.SetTrigger("AttackLight1");
                            //more network animation

                            break;
                        case 2:
                            anim.SetTrigger("AttackLight2");
                            //more network animation
                            break;
                        case 3:
                            anim.SetTrigger("AttackLight3");
                            //more network animation
                            break;
                        default:
                            break;
                        }

                        //only if the weapon is enabled can we lose stamina
                        if(weapon.activeSelf){
                            //lose stamina when we attack
                            CmdChangeStamina(-20.0f);  
                        }
                        lastAttack = Time.time;
                    }
                }
            }
        }
    }

    void FixedUpdate(){
        //        if(!isLocalPlayer){
        //            return;
        //        }
        //        print("anim " + anim.name + " parent " + anim.transform.parent.name);
        //oh great ancestor!
        anim.SetBool("Midair", !this.gameObject.GetComponent<CharacterController>().isGrounded);
        //        if (Physics.Raycast(this.transform.position, -this.transform.parent.transform.up , 1.5f)){
        //
        //        }else{
        //            anim.SetBool ("Midair", false);
        //        }

        InputCheck();

        if(inputs.magnitude > 0){ //makes sure that player cant "run on spot"
            if(Input.GetKey(KeyCode.LeftShift)){
                //stamina code
                if(currentStamina <= 0){
                    //need to change the speed of the walking
                    anim.SetBool("Run", false);
//                    netAnim.SetTrigger("Idle");

                } else{
                    anim.SetBool("Run", true);
                    //netAnim.anim.SetTrigger("Run");

                }
            } else{
                anim.SetBool("Run", false);
//                netAnim.SetTrigger("WalkFoward");

            } 
        }

        if(Input.GetKeyDown(KeyCode.E)){
            anim.SetBool("Interact", true);
        } else if(Input.GetKeyUp(KeyCode.E)){
            anim.SetBool("Interact", false);
        }
    }

    void InputCheck(){
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        inputs = new Vector2(horizontal, vertical);
    }

    [Command]
    void CmdAttack(float damage, GameObject hit){
        if(hit.transform.tag == "Enemy"){
            hit.transform.parent.parent.gameObject.GetComponent<MHealth>().ApplyDamage(damage);

        } else if(hit.transform.tag == "Boss"){
            //hit.transform.parent.parent.gameObject.GetComponent<Health>().ApplyDamage(damage);
            hit.transform.GetComponent<MHealth>().ApplyDamage(damage);
//            Debug.LogError(hit.transform.name);
        } else if(hit.transform.tag == "Player"){//if we hit other players
            hit.transform.gameObject.GetComponent<MHealth>().ApplyDamage(damage);  
        } else{
            
        }
        Debug.LogError("Player hit " + hit.transform.name + " with " + damage + " damage");
    }

    //    [Command]//called from client but run on server
    //    void CmdAttack(int hitterID, float damage, int hitID){
    //        Debug.LogError("Player " + hitterID + " is hitter");
    //        players = GameManager.GetPlayers();
    //        if(players.ContainsKey(hitID)){//if we found the correctly hit object
    //            players[hitID].GetComponent<MHealth>().ApplyDamage(damage);
    //            Debug.LogError("Player " + hitID + " got hit and lost " + damage + " health");
    //        }
    //    }

    [Command]
    public void CmdChangeStamina(float amount){
        if(!isServer){
            return;
        }
        currentStamina += amount;
    }

    void OnChangeStamina(float stamina){
        //updates stamina bar
        currentStamina = stamina;
        if(isLocalPlayer){
            staminabar.GetComponent<MBarScript>().HandleBar();
        }
    }

}