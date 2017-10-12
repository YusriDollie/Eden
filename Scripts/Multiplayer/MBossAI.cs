using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class MBossAI : NetworkBehaviour{

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
    private MHealth health;
    //public Animator anim;
    private float lastAttackTime = 0.0f;
    private GameObject[] players;
    private Vector3 direction;
    private float currRange;
    private float damage;
    [SerializeField] private float attackTime = 75f;
    [SerializeField] private float delayTime = 0.5f;
	[SerializeField] GameObject AOE;
	[SerializeField] GameObject Slash;
    //uncomment this for health
	//LineRenderer line;
    //animations
    public NetworkAnimator netAnim;

    void Awake(){
		
        players = GameObject.FindGameObjectsWithTag("Player");
        //get all players in the session used for boss targeting
		//line = GetComponent<LineRenderer>();
			//line.SetVertexCount(2);
			//line.SetWidth(0.01f, 0.01f);
        int randomint = Random.Range(0, players.Length);
			
        agent = GetComponent<NavMeshAgent>();
        raycastLayer = 1 << LayerMask.NameToLayer("RemoteLayer");//i think since he is server based
        myTransform = transform;
        health = this.GetComponent<MHealth>();
        //first target is chosen randomly
        targetTransform = players[randomint].transform;
		currRange = mAttackRange;
		damage = 30;
        StartCoroutine(activate());
        
        //mAttackRange = 10f;
        //rAttackRange = 100f;
        //jAttackRange = 100f;
        //original = transform.position;
        //playerInSight = false;
        //anim = this.GetComponentInChildren<Animator>();
        //anim.SetBool ("WalkForward", true);
    }




    void phaseTransition(){

        if(currentPhase == 1){
            //transition to next phase with animation
            netAnim.animator.SetTrigger("Taunt1");

        }
        if(currentPhase == 2){
            //transition to next phase with animation
            netAnim.animator.SetTrigger("Taunt2");

        }
    }



    bool phaseTrigger(){

        //run in CoRoutine?
        if(currentPhase == phases[0]){
            if(health.currentHealth <= 66 && health.currentHealth > 33){
                currentPhase = phases[1];
                currRange = rAttackRange;
                damage = 40;
                delayTime = 2f;
                //phaseTransition();
                Debug.LogError("Transform 1");
                return true;
            } else{
                return false;
            }
        } else if(currentPhase == phases[1]){
            if(health.currentHealth <= 33 && health.currentHealth > 0){
                currentPhase = phases[2];
                currRange = jAttackRange;
                damage = 10;
                delayTime = 3f;
                //phaseTransition();
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
        try{
            distance = Vector3.Magnitude(targetTransform.position - myTransform.position);
            direction = Vector3.Normalize(targetTransform.position - myTransform.position);

            if(distance >= 30f){

                targetTransform = switchTarget();
            }

        } catch(MissingReferenceException){

            targetTransform = switchTarget();
        }
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
        var temp = (lastAttackTime + attackTime) / 1000.0f;
        //print ("I did it at " +temp);

        if(Time.time >= temp){
            //Do Attack stuff here
            //Debug.LogError("bitches");
            // player.gameObject.GetComponent<Health>().ApplyDamage(damage);



            lastAttackTime = Time.time;
            if(currentPhase == 0){
                //melee combo here
                netAnim.animator.SetTrigger("Attack1");
                Target();
                //StartCoroutine(attackAnim(1));

                //hit();
                Debug.LogError("Boss Hit 1");

            } else if(currentPhase == 1){

                //shoot then transiton to hit combo
                netAnim.animator.SetTrigger("Attack2");
                //StartCoroutine(attackAnim(2));
                //GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                //sphere.transform.position = myTransform.position;
				Instantiate(AOE,myTransform.position,Quaternion.Euler(90f,0f,90f));
                //Destroy(sphere, 2f);
				//Destroy(AOE,1f);
                AoEHit();
                Debug.LogError("Boss AoE 2");


            } else if(currentPhase == 2){

                //jump onto player current location then melee
				netAnim.animator.SetTrigger("Attack3");
				Instantiate(Slash,myTransform.position,myTransform.rotation);
                //StartCoroutine(attackAnim(3));
                //add a charge delay and target.
                Target();
                //add delay here to allow player to dodge the raycast, make it like a lazer beam or something
                Shoot();
                Debug.LogError("Boss Shoot 3");

            }
        }

    }
    /*
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
*/
    //NOTE!!!! Hit calls all atttacks it looks up the corresponding range and damage for each attack then raycasts accordingly before applying the correct damage


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
                hit.transform.gameObject.GetComponent<MHealth>().ApplyDamage(damage);
            }
        }

    }


    void Shoot(){

        RaycastHit hit;


		if(Physics.Raycast(myTransform.position, direction, out hit, currRange, -1)){//using -1 to hit on all layers [temporary]

			//line.SetPosition(1, hit.point);
            if((hit.transform.tag == "Player")){
                //Debug.LogError("Found something to hit! that something is " + hit.transform.name);                            //run atack command passing in hitter and hit netid's and the amount of damage depending on the combo
                //play punch sound
                //audioSource.PlayOneShot(meleeSounds[0]);
                //Attack(mel.comboDamage[comboPos], hit);
                hit.transform.gameObject.GetComponent<MHealth>().ApplyDamage(damage);
            }
        }

    }



    void AoEHit(){
        Collider[] searchColliders = Physics.OverlapSphere(myTransform.position, rAttackRange, -1);

        if(searchColliders.Length > 0){

            foreach(Collider x in searchColliders){

                if(x.gameObject.tag == "Player"){

                    x.gameObject.GetComponent<MHealth>().ApplyDamage(damage);
                    break;
                }

            } 


        }

    }


    //method returns with new AI targer
    Transform switchTarget(){
        Transform tmin = null;
        float minDist = Mathf.Infinity; //impossibly high number
        foreach(GameObject x in players){
            if(x.GetComponent<MHealth>().deathCount < 5){
                float dist = Vector3.Distance(x.transform.position, myTransform.position);
                if(dist < minDist){


                    tmin = x.transform;
                    minDist = dist;
                }



            }


        }

        return tmin;

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
