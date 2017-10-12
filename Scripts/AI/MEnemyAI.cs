using UnityEngine;
using System.Collections;
using UnityEngine.Networking;


public class MEnemyAI : NetworkBehaviour{

    public float fieldOfViewAngle = 110f;
    // Number of degrees, centred on forward, for the enemy see.
    public bool playerInSight;
    // Whether or not the player is currently sighted.
    private Vector3 LastSighting;
    //Last seen player position
    private Transform previousSighting;

    public bool stationary = false;

    private NavMeshAgent agent;
    private Vector3 original;
    private Transform myTransform;
    public Transform targetTransform;
    private LayerMask raycastLayer;
    public float chaseSpeed = 5f;
    public float patrolSpeed = 2f;
    public float attackRange = 1f;


    //Change raius to change aggro range
    //enemies maintain combat while player is in aggro range chasing across map while range maintained, once range is broken they return to original location and behaviour
    //similar to darksouls mechanics

    private float radius = 25;
    private Vector3 direction;
    private float dot;
    //public GameObject[] players;
    // Use this for initialization
    //LineRenderer line;
    void Awake(){

        //  line = GetComponent<LineRenderer>();
        //  line.SetVertexCount(2);
        //  line.SetWidth(0.01f, 0.01f);


    }

    void Start(){
        agent = GetComponent<NavMeshAgent>();

        //agent.isOnNavMesh
        raycastLayer = 1 << LayerMask.NameToLayer("LocalPlayer");
        myTransform = transform;
        original = transform.position;
        playerInSight = false;
        //line.SetPosition (0,myTransform.position);
        //players = GameObject.FindGameObjectsWithTag("Player");
        if(isServer){
            StartCoroutine(stateCheck());  
        }
    }


    void MoveToTarget(){

        if(!isServer){
            return;
        }

        //agent.SetDestination (targetTransform.position);




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
        agent.SetDestination(original);
        playerInSight = false;

    }


    // Update is called once per frame
    void Update(){

    }


    bool searchTrigger(){
        //may need to check if client

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

    void search(){

        //temp
        if(!isServer){
            return;
        }

        //playerInSight = false;
        dot = Vector3.Dot(direction, Vector3.forward);
        float angle = Vector3.Angle(direction, transform.forward);
        Debug.LogError(angle < fieldOfViewAngle * 0.5f);
        if(angle < fieldOfViewAngle * 0.5f){
            RaycastHit hit;
            //print ("LaserBeam!!");
            // ... and if a raycast towards the player hits something...

            if(Physics.Raycast(myTransform.position, direction, out hit, 50)){

                //print ("Me : "+ transform.position);
                //print ("Player: " + targetTransform.position);
                //  print("Vector: "+ direction);

                //line.SetPosition (1, hit.point);
                //print (hit.transform.gameObject.tag);
                // ... and if the raycast hits the player...
                //print(targetTransform.tag);
                //print(hit.transform.gameObject.name);
                if(hit.transform.gameObject.tag == "Player"){
                    // ... the player is in sight.
                    playerInSight = true;
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
        if(!isServer){
            return;
        }

        dot = Vector3.Dot(direction, Vector3.forward);
        float angle = Vector3.Angle(direction, transform.forward);
        Debug.LogError(angle < fieldOfViewAngle * 0.5f);
        if(angle < fieldOfViewAngle * 0.5f){
            RaycastHit hit;
            //print ("LaserBeam!!");
            // ... and if a raycast towards the player hits something...

            if(Physics.Raycast(myTransform.position, direction, out hit, 50)){

                //print ("Me : "+ transform.position);
                //print ("Player: " + targetTransform.position);
                //  print("Vector: "+ direction);

                //line.SetPosition (1, hit.point);
                //print (hit.transform.gameObject.tag);
                // ... and if the raycast hits the player...
                Debug.LogError(targetTransform.tag);
                Debug.LogError(hit.transform.gameObject.name);
                if(hit.transform.gameObject.tag == "Player"){
                    // ... the player is in sight.
                    playerInSight = true;
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

    //probably will be a command
    void attack(){


        //Do Attack stuff here

        //for now print

        Debug.LogError("attacking");
    }


    IEnumerator stateCheck(){

        //while alive infinite for now, death needs to break loop
        for(;;){

            if(!stationary){//non - stationary  = goes towards you
                if(searchTrigger()){//are we searching for targets? - is something in aggro range
                    search();//randomly moves in the navmesh for 50 units and move there [search pattern]
                    MoveToTarget();//goes to last known position

                    if(Vector3.Distance(myTransform.position, targetTransform.position) < attackRange){

                        attack();
                    }

                } else{

                    idle();
                }

                yield return new WaitForSeconds(0.2f);
            } else{
                //stationary response message
                if(searchTrigger()){
                    jumpbot();//stationary AI - wait until insight then attack
                    MoveToTarget();
                }

                yield return new WaitForSeconds(0.2f);
            }
        }



    }


}
