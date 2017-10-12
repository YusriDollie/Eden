using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class EnemyLocationSpawn : MonoBehaviour{

    private GameObject[] enemies;
    List<GameObject> tiggers;
    private Collider playerCollider;
    private GameObject player;
    private Bounds bounds;
    private BoxCollider box;


    // Use this for initialization
    void Start(){
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        //collider = gameObject.GetComponent<BoxCollider> (); 
        box = gameObject.GetComponent<BoxCollider>(); 
        bounds = box.bounds;
        tiggers = new List<GameObject>();
        player = GameObject.FindGameObjectWithTag("Player");
        //Debug.LogError (player.name);
        playerCollider = player.GetComponent <Collider>();
        //playerCollider.get
        //	Debug.LogError (playerCollider.GetType());
        foreach(GameObject x in enemies){
            if(bounds.Contains(x.transform.position) && (x.name != "SecuroDrone")){
                //print (x.name);
                tiggers.Add(x);
            }

        }

    }
	
    // Update is called once per frame
    void Update(){

	
	
    }

    void OnTriggerEnter(Collider collider){
        //Destroy(other.gameObject);
        //print(playerCollider.name);
        if(collider.tag == "Player"){
            print("COLLISON");
            foreach(GameObject x in tiggers){
                //print ("enabling");

                //Debug.LogError(x.name);
                //var test = x.GetComponent<EntityAI> ();//.enabled = true;

                x.GetComponent<NavMeshAgent>().enabled = true;
                x.GetComponent<EntityAI>().enabled = true;
                //Debug.LogError(test.isActiveAndEnabled);
                //x.GetComponent<NavMeshAgent> ().enabled = true;
                //print ("Done");
                Destroy(this.gameObject);
            }
        }

    }


}