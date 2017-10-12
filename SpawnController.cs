using UnityEngine;
using System.Collections;

public class SpawnController : MonoBehaviour{
    private SpawnManager player;
	[SerializeField] private Material white;
	[SerializeField] private GameObject obj;
    // Use this for initialization
    void Start(){
    }
	
    // Update is called once per frame
    void Update(){
	
    }

    void OnTriggerEnter(Collider col){
        if(col.tag == "Player"){
            player = col.GetComponent<SpawnManager>();
            player.currentSpawnPoint = int.Parse(transform.GetChild(0).name);
            Debug.Log("Set spawn point to " + transform.GetChild(0).name);
			obj.GetComponent<Renderer> ().material = white;
        }
    }
}
