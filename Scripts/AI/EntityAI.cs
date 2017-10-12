using UnityEngine;
using System.Collections;
using UnityEngine.Networking;


//public class EntityAI : NetworkBehaviour {
public class EntityAI : MonoBehaviour{

    public float fieldOfViewAngle = 110f;
    // Number of degrees, centred on forward, for the enemy see.
    public bool playerInSight;
    // Whether or not the player is currently sighted.
    private Vector3 LastSighting;
    //Last seen player position
    private GameObject player;
    private Transform previousSighting;
    private GameObject[] Prisoners;
    private float damage = 15.0f;

    public bool stationary = false;

    private NavMeshAgent agent;
    private Vector3 original;
    private Transform myTransform;
    public Transform targetTransform;
    private LayerMask raycastLayer;
    public float chaseSpeed = 5f;
    public float patrolSpeed = 2f;
    public float attackRange = 2f;
    private float distance;
    //private Collider fist;

    //Change raius to change aggro range
    //enemies maintain combat while player is in aggro range chasing across map while range maintained, once range is broken they return to original location and behaviour
    //similar to darksouls mechanics

    private float radius = 25;
    private Vector3 direction;
    private float dot;
    //public GameObject[] players;
    // Use this for initialization
    //LineRenderer line;

    //750 milliseconds between each attack
    private float lastAttackTime = 0.0f;
    [SerializeField] private float attackTime = 75f;

    public static ArrayList targets;

    private Animator anim;
    [SerializeField] private GameObject bot;

    void Awake(){
        //	line = GetComponent<LineRenderer>();
        //	line.SetVertexCount(2);
        //	line.SetWidth(0.01f, 0.01f);
        //fist = GetComponentInChildren<BoxCollider>();
    }

    void Start(){
        player = GameObject.FindGameObjectWithTag("Player");
        Prisoners = GameObject.FindGameObjectsWithTag("Prisoner");
        agent = GetComponent<NavMeshAgent>();
        raycastLayer = 1 << LayerMask.NameToLayer("PlayerDetCollider");
        myTransform = transform;
        original = transform.position;
        playerInSight = false;
        //line.SetPosition (0,myTransform.position);
        //players = GameObject.FindGameObjectsWithTag("Player");
        anim = bot.GetComponent<Animator>();
        StartCoroutine(stateCheck());
    }

    void MoveToTarget(){
        anim.SetBool("WalkForward", true);
        if(playerInSight){
            agent.SetDestination(targetTransform.position);
        } else{
            if(LastSighting != null){
                agent.SetDestination(LastSighting);
                for(int x = 0; x < 10; x++){
                    search();
                }
            } else{
                //never been seen yet
            }
        }
    }

    void idle(){
        //do idle stuff like patrol or sit around
        playerInSight = false;
        anim.SetBool("WalkForward", false);
        agent.SetDestination(original);
        anim.SetTrigger("Idle");
    }

    // Update is called once per frame
    void Update(){
        try{
            distance = Vector3.Magnitude(targetTransform.position - myTransform.position);
        } catch(MissingReferenceException){
            targetTransform = null;
            idle();
        }
    }


    bool searchTrigger(){
        Collider[] searchColliders = Physics.OverlapSphere(myTransform.position, radius, raycastLayer);

        if(searchColliders.Length > 0){
			
            //Player has entered aggro range AI needs to establish visual 
            int randomint = Random.Range(0, searchColliders.Length);
            targetTransform = searchColliders[randomint].transform;
            direction = Vector3.Normalize(targetTransform.position - myTransform.position);
            return true;
        } else{
            return false;
        }


    }


    bool jumpTrigger(){
        //Debug.LogError (gameObject.name);
        direction = Vector3.Normalize(targetTransform.position - myTransform.position);
        float angle = Vector3.Angle(direction, transform.forward);
        //  print(angle < fieldOfViewAngle * 0.5f);
        if(angle < fieldOfViewAngle * 0.5f){
            RaycastHit hit;
            //print ("LaserBeam!!");
            // ... and if a raycast towards the player hits something...
            if(Physics.Raycast(myTransform.position, direction, out hit, 50)){

                if(hit.transform.gameObject.tag == "Player"){
                    // ... the player is in sight.

                    playerInSight = true;
                    //sight overrides random sense
                    targetTransform = hit.transform;
                    //print ("Seen");
                    // Set the last global sighting is the players current position.
                    LastSighting = targetTransform.position;
                    return true;
                } else{
                    playerInSight = false;
                }
            }
        }

        return false;

    }

    void search(){


        //playerInSight = false;
        //  dot = Vector3.Dot(direction, Vector3.forward);
        float angle = Vector3.Angle(direction, transform.forward);
        //  print(angle < fieldOfViewAngle * 0.5f);
        if(angle < fieldOfViewAngle * 0.5f){
            RaycastHit hit;
            //print ("LaserBeam!!");
            // ... and if a raycast towards the player hits something...
            if(Physics.Raycast(myTransform.position, direction, out hit, 50)){

                if(hit.transform.gameObject.tag == "Player" || hit.transform.gameObject.tag == "Prisoner"){
                    // ... the player is in sight.

                    playerInSight = true;
                    //sight overrides random sense
                    targetTransform = hit.transform;
                    //print ("Seen");
                    // Set the last global sighting is the players current position.
                    LastSighting = targetTransform.position;
                } else{
                    playerInSight = false;
                }
            }
        }


        if(!playerInSight){
            //print ("I cant see");

            Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * radius;
            randomDirection += myTransform.position;
            NavMeshHit navHit;
            NavMesh.SamplePosition(randomDirection, out navHit, radius, -1);
            agent.SetDestination(navHit.position);
        }
    }

    void jumpbot(){
		
        // dot = Vector3.Dot(direction, Vector3.forward);
        float angle = Vector3.Angle(direction, transform.forward);
        //  print(angle < fieldOfViewAngle * 0.5f);
        if(angle < fieldOfViewAngle * 0.5f){
            RaycastHit hit;
            //print ("LaserBeam!!");
            // ... and if a raycast towards the player hits something...

            if(Physics.Raycast(myTransform.position, direction, out hit, 50)){

                if(hit.transform.gameObject.tag == "Player"){
                    // ... the player is in sight.
                    playerInSight = true;
                    targetTransform = hit.transform;
                    //attempt attack maybe?


                    // Set the last global sighting is the players current position.
                    LastSighting = targetTransform.position;
                }
            }
        }

        if(!playerInSight){
            //do The idle look around stuff
            //idle ();
        }
    }


    bool CanAttack(){
        if(distance <= attackRange){
            agent.SetDestination(myTransform.position);
            return true;
        } else{
            return false;
        }
    }


    void hit(){

        RaycastHit hit;

        if(Physics.Raycast(myTransform.position, myTransform.forward.normalized, out hit, 1.1f, -1)){//using -1 to hit on all layers [temporary]

            //line.SetPosition(1, hit.point);
            if((hit.transform.tag == "Player") || (hit.transform.tag == "Prisoner")){
                //Debug.LogError("Found something to hit! that something is " + hit.transform.name);                            //run atack command passing in hitter and hit netid's and the amount of damage depending on the combo
                //play punch sound
                //audioSource.PlayOneShot(meleeSounds[0]);
                //Attack(mel.comboDamage[comboPos], hit);
                hit.transform.gameObject.GetComponent<Health>().ApplyDamage(damage);
            }
        }

    }

    void attack(){
        //print (Time.time);
        myTransform.LookAt(targetTransform.position);
        var temp = (lastAttackTime + attackTime) / 1000;
        //print ("I did it at " +temp);

        if(Time.time >= temp){
            //Do Attack stuff here
            //Debug.LogError("bitches");
            // player.gameObject.GetComponent<Health>().ApplyDamage(damage);

		

            lastAttackTime = Time.time;
            switch((int)(Random.Range(0, 3))){
            case 0:
                anim.SetTrigger("Attack1");
                hit();
				//WaitForSeconds (2f);
                break;
            case 1:
                anim.SetTrigger("Attack2");
                hit();
                break;
            case 2:
                anim.SetTrigger("Attack3");
                hit();
                break;
            default:
                break;
            }
        }
        //for now print
        //Debug.LogError("attacking",this);
        //print("attacking");
    }

    /*
	void OnTriggerEnter(Collider collider) {
		//Destroy(other.gameObject);
		//print(playerCollider.name);
		Debug.LogError("FIST");
		if (collider.tag == "Player") {
			//print("COLLISON");
			collider.gameObject.GetComponent<Health>().ApplyDamage(damage);
		
		}

	}*/


    IEnumerator stateCheck(){

        //while alive infinite for now, death needs to break loop
        for(;;){

            if(!stationary){
                if(searchTrigger()){
                    search();
                    MoveToTarget();

                    //play attacking music
                    if(targetTransform != null && targetTransform.tag == "Player"){
                        if(targetTransform.GetComponent<AudioManager>().currentTrack != 2){
                            targetTransform.GetComponent<AudioManager>().PlayTrack(2);
                        }
                    }

                    if(CanAttack()){
                        //agent.SetDestination (myTransform.position);
                        attack();

                        yield return new WaitForSeconds(attackTime / 1000);
                    }

                } else{
                    idle();

                }

                yield return new WaitForSeconds(0.2f);
            } else{
                //stationary response message
                targetTransform = player.transform;
                if(jumpTrigger()){
                    //jumpbot();
                    MoveToTarget();
                    //play attacking music
                    if(targetTransform != null && targetTransform.tag == "Player"){
                        if(targetTransform.GetComponent<AudioManager>().currentTrack != 2){
                            targetTransform.GetComponent<AudioManager>().PlayTrack(2);
                        }
                    }

                    if(CanAttack()){
                        //agent.SetDestination (myTransform.position);
                        attack();

                        yield return new WaitForSeconds(attackTime / 1000);
                    }

                } else{
                    idle();
                }


            }




            yield return new WaitForSeconds(0.2f);
        }
    }



}


