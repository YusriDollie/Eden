using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class BossRoomController : MonoBehaviour{

    [SerializeField] GameObject floor;
    [SerializeField] GameObject bossMan;
    [SerializeField] GameObject bossManDummy;
    [SerializeField] GameObject explosion;
    private Health bossHealth;
    [SerializeField] GameObject EndGameSplash;
    [SerializeField] GameObject deaths;
    [SerializeField] GameObject timer;
    [SerializeField] GameObject player;

    public GameObject blackCube;

    // Use this for initialization
    void Start(){
        bossHealth = bossMan.GetComponent<Health>();
    }
	
    // Update is called once per frame
    void FixedUpdate(){
        if((bossHealth.currentHealth <= 0) || (bossMan == null)){	//the null is for when he gets deleted...we should really ragdoll him
            SceneManager.LoadScene("WinScene");
        }
    }

    void OnCollisionEnter(Collision col){
        if(col.gameObject.tag == "Boss"){
            floor.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider col){
        if(col.gameObject.tag == "Player"){
            Debug.Log("boop");
            //bossMan.transform.localPosition = new Vector3 (2.68f, 9.35f, 2.3f);
            bossMan.GetComponent<NavMeshAgent>().enabled = true;
            bossMan.GetComponent<BossAI>().enabled = true;
            bossManDummy.SetActive(true);
        }
        if(col.gameObject.tag == "Boss"){
            Destroy(blackCube);
            floor.SetActive(false);
            Destroy(col.gameObject);
            explosion.SetActive(true);
            GameObject.FindGameObjectWithTag("Player").GetComponent<AudioManager>().PlayTrack(4);
        }
    }
}
