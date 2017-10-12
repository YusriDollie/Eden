using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour{
    public GameObject[] spawnPoints;
    public int currentSpawnPoint;
    //sound effects
    [SerializeField] public AudioClip[] spawnSounds;
    public AudioSource audioSource;

    // Use this for initialization
    void Start(){
        //spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        audioSource = GetComponent<AudioSource>();
    }
	
    // Update is called once per frame
    void Update(){
        //update if spawn point is crossed - currentSpawn ++;
    }


    public void Respawn(){
        audioSource.PlayOneShot(spawnSounds[0]);
        Vector3 spawnPoint = Vector3.zero;
        if(spawnPoints != null && spawnPoints.Length > 0){
            spawnPoint = spawnPoints[currentSpawnPoint - 1].transform.GetChild(0).transform.position;
            this.transform.position = spawnPoint;
            GetComponent<AudioManager>().PlayTrack(0); 
            //transform.position = Vector3.zero;
        }
    }
}
