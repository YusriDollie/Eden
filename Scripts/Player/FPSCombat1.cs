using UnityEngine;
using System.Collections.Generic;
using UnityStandardAssets.Characters.FirstPerson;


public class FPSCombat1 : MonoBehaviour{

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
    private const float maxStamina = 100.0f;
    public float currentStamina = maxStamina;
    public float staminRegenRate = 4.0f;

    private CharacterController chController;
    private FirstPersonController fpController;
    private WeaponHandler1 weaponHandler;

    //sound effects
    [SerializeField] private AudioClip[] meleeSounds;
    private AudioSource audioSource;

    //audio manager
    private AudioManager audioMan;

    //layer mask to control what the ray hits i.e. other players
    [SerializeField] private LayerMask mask;

    // Use this for initialization
    void Awake(){
        weaponHandler = GetComponent<WeaponHandler1>();
        InitWeapon(weaponHandler.curWeapon);
        cam = Camera.main;
        chController = GetComponent<CharacterController>();
        fpController = GetComponent<FirstPersonController>();
        audioSource = GetComponent<AudioSource>();
        audioMan = GetComponent<AudioManager>();
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
            ChangeStamina(Time.deltaTime * -10.0f);
        }

        //stamina regeneration all the time including if we aren't running
        if(!Input.GetKey(KeyCode.LeftShift) && (currentStamina >= 0)){
            ChangeStamina(Time.deltaTime * staminRegenRate);
        }

        //if stamina is fully regenerated
        if(currentStamina >= maxStamina){
            currentStamina = maxStamina;
        }

        //if stamina is empty
        if(currentStamina <= 0){
            currentStamina = 0;
        }

        //audio control code

        if(Input.GetKeyDown(KeyCode.Minus)){
            audioMan.PlayTrack(audioMan.currentTrack - 1);
        }
        if(Input.GetKeyDown(KeyCode.Equals)){
            audioMan.PlayTrack(audioMan.currentTrack + 1);
        }

        //end audio control code

        if(Input.GetButtonDown("Fire1")){
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
                        if((hit.transform.tag == "Enemy") || (hit.transform.tag == "BreakableObject") || (hit.transform.tag == "Boss")){
                            //Debug.LogError("Found something to hit! that something is " + hit.transform.name);                            //run atack command passing in hitter and hit netid's and the amount of damage depending on the combo
                            //play punch sound
                            audioSource.PlayOneShot(meleeSounds[0]);
                            Attack(mel.comboDamage[comboPos], hit);

                            //I added this to make the AI aggro immediatly onto anything that attacks them if they dont see you yet, this also makes stationary AI more realistic- yusri
                            if(hit.transform.tag == "Enemy"){

                                if(hit.transform.parent.parent.GetComponent<EntityAI>().playerInSight == false){

                                    hit.transform.parent.parent.transform.LookAt(this.gameObject.transform);
                                    hit.transform.parent.parent.GetComponent<EntityAI>().targetTransform = this.gameObject.transform;
                                    hit.transform.parent.parent.GetComponent<EntityAI>().playerInSight = true;
                                }


                            }

                        } else{
                            if(hit.transform.name != null){//not sure if this is necessary
                                Debug.LogError("hit " + hit.transform.name);
                            } else{
                                Debug.LogError("hit nothing");
                              
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
                    //RaycastHit rayhit = Physics.Raycast
                        break;
                    case 1:
                        anim.SetTrigger("AttackLight1");
                        break;
                    case 2:
                        anim.SetTrigger("AttackLight2");
                        break;
                    case 3:
                        anim.SetTrigger("AttackLight3");
                        break;
                    default:
                        break;
                    }

                    //only if the weapon is enabled can we lose stamina
                    if(weapon.activeSelf){
                        //lose stamina when we attack
                        ChangeStamina(-20.0f);  
                    }
                    lastAttack = Time.time;
                } 
            }
        }

//        print("Stamina " + currentStamina);

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
                } else{
                    anim.SetBool("Run", true);
                }
            } else{
                anim.SetBool("Run", false);
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

    void Attack(float damage, RaycastHit hit){
        if(hit.transform.gameObject.tag == "Enemy"){
            hit.transform.parent.parent.gameObject.GetComponent<Health>().ApplyDamage(damage);

        } else if(hit.transform.gameObject.tag == "Boss"){
            //hit.transform.parent.parent.gameObject.GetComponent<Health>().ApplyDamage(damage);
			hit.transform.parent.parent.GetComponent<Health>().ApplyDamage(damage);
            Debug.LogError(hit.transform.name);
        } else if(hit.transform.gameObject.tag == "BreakableObject"){
            hit.transform.gameObject.GetComponent<VentController>().Break();
        } else{
            hit.transform.gameObject.GetComponent<Health>().ApplyDamage(damage);

        }        
        Debug.LogError("Player hit " + hit.transform.name + " with " + damage + " damage");
    }

    public void ChangeStamina(float amount){
        currentStamina += amount;
    }
}