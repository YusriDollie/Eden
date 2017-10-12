using UnityEngine;
using System.Collections;

public class BossAI : MonoBehaviour{

    //Need the list of all players in the session for network play,
    //then replace the singleplayer with a selection from that list
    //public GameObject[] players;
    private NavMeshAgent agent;
    //private Vector3 original;
    private Transform myTransform;
    public Transform targetTransform;
    private LayerMask raycastLayer;
    private GameObject player;
    private float distance;
    public float mAttackRange;
    public float rAttackRange;
    public float jAttackRange;
    private int[] phases = new int[] { 0, 1, 2 };
    public int currentPhase;
    private Health health;
    public Animator anim;
    private float lastAttackTime = 0.0f;
    private Vector3 direction;
    private float currRange;
    private float damage;
    [SerializeField] private float attackTime = 75f;
    [SerializeField] private float delayTime = 0.5f;
	[SerializeField] GameObject AOE;
	[SerializeField] GameObject Slash;
    //LineRenderer line;
    //uncomment this for health

    float maxHealth;


    void Awake(){

        //	line = GetComponent<LineRenderer>();
        //	line.SetVertexCount(2);
        //	line.SetWidth(0.01f, 0.01f);

        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        raycastLayer = 1 << LayerMask.NameToLayer("PlayerDetCollider");
        myTransform = transform;
        health = this.GetComponent<Health>();
        targetTransform = player.transform;
		currRange = mAttackRange;
		damage = 30;
		maxHealth = GetComponent<Health>().getMaxHealth();
        StartCoroutine(activate());
       

        //mAttackRange = 3f;
        //rAttackRange = 5f;
        //jAttackRange = 10f;
        //original = transform.position;
        //playerInSight = false;
        //anim = this.GetComponentInChildren<Animator>();
        //anim.SetBool ("WalkForward", true);
    }




    void phaseTransition(){

        if(currentPhase == 1){
            //transition to next phase with animation
			anim.SetTrigger("Taunt1");

        }
        if(currentPhase == 2){
            //transition to next phase with animation
			anim.SetTrigger("Taunt2");

        }
    }



    bool phaseTrigger(){

        //run in CoRoutine?
        if(currentPhase == phases[0]){
            if((health.currentHealth / maxHealth) * 100.0f <= 66.6f && (health.currentHealth / maxHealth) * 100.0f > 33.3f){
                currentPhase = phases[1];
                currRange = rAttackRange;
                damage = 40;
                delayTime = 2f;
                //phaseTransition();
                player.GetComponent<AudioManager>().PlayTrack(5);
                Debug.LogError("Transform 1");
                return true;
            } else{

                return false;
            }
        } else if(currentPhase == phases[1]){

            if((health.currentHealth / maxHealth) * 100.0f <= 33.3f && (health.currentHealth / maxHealth) * 100.0f > 0.0f){
                currentPhase = phases[2];
                currRange = jAttackRange;
                damage = 10;
                delayTime = 1f;
                //phaseTransition();
                player.GetComponent<AudioManager>().PlayTrack(6);
                Debug.LogError("Transform 2");
                return true;

            } else{

                return false;
            }
        } else{

            return false;
        }


    }







    // Use this for initialization
    void Start(){
        currentPhase = phases[0]; 
    }
	
    // Update is called once per frame
    void Update(){
        distance = Vector3.Magnitude(targetTransform.position - myTransform.position);
        direction = Vector3.Normalize(targetTransform.position - myTransform.position);
    }


    void Target(){
        //Debug.LogError(this.name);
        myTransform.LookAt(targetTransform.position);
        if(distance > currRange){
            agent.SetDestination(targetTransform.position);
        }
    }


    bool CanAttack(){

        if(currentPhase == 0){

            if(distance < mAttackRange){

                agent.SetDestination(myTransform.position);
                return true;
            } else{


                return false;
            }

        } else if(currentPhase == 1){

            if(distance < rAttackRange){

                agent.SetDestination(myTransform.position);
                return true;
            } else{


                return false;
            }

        } else if(currentPhase == 2){

            if(distance < jAttackRange){

                agent.SetDestination(myTransform.position);
                return true;
            } else{


                return false;
            }

        }

        return false;
    }


    void Attack(){
        //need to make it cycle through its attacks, jacques get on that
        var temp = (lastAttackTime + attackTime) / 1000;
        //print ("I did it at " +temp);

        if(Time.time >= temp){
            //Do Attack stuff here
            //Debug.LogError("bitches");
            // player.gameObject.GetComponent<Health>().ApplyDamage(damage);



            lastAttackTime = Time.time;
            if(currentPhase == 0){
                //melee combo here
                anim.SetTrigger("Attack1");
                //StartCoroutine(attackAnim(1));
                Target();
                //hit();
                Debug.LogError("Boss Hit 1");

            } else if(currentPhase == 1){

                //shoot then transiton to hit combo
                anim.SetTrigger("Attack2");
                //StartCoroutine(attackAnim(2));
				Instantiate(AOE,myTransform.position,Quaternion.Euler(90f,0f,90f));
				//Destroy(sphere, 2f);
				//Destroy(AOE,1f);
				//AoEHit();
                AoEHit();
                Debug.LogError("Boss AoE 2");


            } else if(currentPhase == 2){

                //jump onto player current location then melee
                anim.SetTrigger("Attack3");
                //StartCoroutine(attackAnim(3));
				Instantiate(Slash,myTransform.position,myTransform.rotation);
                //add a charge delay and target.
                Target();
                Shoot();
                Debug.LogError("Boss Shoot 3");

            }
        }

    }

    IEnumerator attackAnim(int i){
        switch(i){
        case 1:
            Debug.Log("Attack1");
            anim.SetTrigger("Attack1");
            yield return new WaitForSeconds(1f);
            break;
        case 2:
            Debug.Log("Attack2");
            anim.SetTrigger("Attack2");
            yield return new WaitForSeconds(1f);
            break;
        case 3:
            Debug.Log("Attack3");
            anim.SetTrigger("Attack3");
            yield return new WaitForSeconds(1f);
            break;
        default:
            yield return null;
            break;
        }

        yield return null;
    }



    //NOTE!!!! Hit calls all atttacks it looks up the corresponding range and damage for each attack then raycasts accordingly before applying the correct damage

    void hit(){

        RaycastHit hit;

        if(Physics.Raycast(myTransform.position, direction, out hit, currRange, -1)){//using -1 to hit on all layers [temporary]

            //line.SetPosition(1, hit.point);
            if((hit.transform.tag == "Player")){
                //Debug.LogError("Found something to hit! that something is " + hit.transform.name);                            //run atack command passing in hitter and hit netid's and the amount of damage depending on the combo
                //play punch sound
                //audioSource.PlayOneShot(meleeSounds[0]);
                //Attack(mel.comboDamage[comboPos], hit);
                hit.transform.gameObject.GetComponent<Health>().ApplyDamage(damage);
            }
        }

    }


    void Shoot(){

        RaycastHit hit;

        if(Physics.Raycast(myTransform.position, myTransform.forward.normalized, out hit, currRange, -1)){//using -1 to hit on all layers [temporary]

            //line.SetPosition(1, hit.point);
            if((hit.transform.tag == "Player")){
                //Debug.LogError("Found something to hit! that something is " + hit.transform.name);                            //run atack command passing in hitter and hit netid's and the amount of damage depending on the combo
                //play punch sound
                //audioSource.PlayOneShot(meleeSounds[0]);
                //Attack(mel.comboDamage[comboPos], hit);
                hit.transform.gameObject.GetComponent<Health>().ApplyDamage(damage);
            }
        }

    }



    void AoEHit(){
        Collider[] searchColliders = Physics.OverlapSphere(myTransform.position, rAttackRange, -1);

        if(searchColliders.Length > 0){

            foreach(Collider x in searchColliders){
				
                if(x.gameObject.tag == "Player"){
					
                    x.gameObject.GetComponent<Health>().ApplyDamage(damage);
					break;
                }

            } 


        }

    }



    IEnumerator activate(){

        //While alive, infinite for now
        for(;;){
            //Target ();
            if(phaseTrigger()){

                phaseTransition();

            }


            if(CanAttack()){
                //Target ();
                //myTransform.LookAt (targetTransform.position);
                Attack();
                yield return new WaitForSeconds(delayTime);
            } else{

                Target();
            }

            //	Target ();
            //	Attack ();
            yield return new WaitForSeconds(0.2f);
        }

    }
}
