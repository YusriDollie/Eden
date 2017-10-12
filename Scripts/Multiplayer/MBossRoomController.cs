using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using Prototype.NetworkLobby;
using UnityStandardAssets.CrossPlatformInput;

public class MBossRoomController : NetworkBehaviour{

    [SerializeField] GameObject bossManPrefab;
    private MHealth bossHealth;
    [SerializeField] GameObject EndGameSplash;
    [SerializeField] GameObject deaths;
    [SerializeField] GameObject timer;
    [SerializeField] GameObject player;

    private GameObject bossMan;
    private bool bossSpawned = false;

    // Update is called once per frame
    void FixedUpdate(){
        if(bossSpawned){
            if((bossHealth.currentHealth <= 0) || (bossMan == null)){   //the null is for when he gets deleted...we should really ragdoll him
                StartCoroutine(WinningScreen());
            }
        }
    }

    void OnTriggerEnter(Collider col){
        if(col.gameObject.tag == "Player"){
            Debug.Log("boop");
            bossMan = (GameObject)Instantiate(bossManPrefab, bossManPrefab.transform.position, bossManPrefab.transform.rotation);
//            bossMan.transform.Position = new Vector3(2.68f, 9.35f, 2.3f);
            NetworkServer.Spawn(bossMan);
            bossHealth = bossMan.GetComponent<MHealth>();
            bossMan.SetActive(true);
            bossMan.GetComponent<NavMeshAgent>().enabled = true;
            bossMan.GetComponent<MBossAI>().enabled = true;
            GetComponent<BoxCollider>().enabled = false;
            bossSpawned = true;

        }
    }

    IEnumerator WinningScreen(){
        yield return new WaitForSeconds(2.0f);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        GameObject lobby = GameObject.FindGameObjectWithTag("LobbyManager");
        if(lobby){
            lobby.GetComponent<LobbyManager>().StopHostClbk();
        }
        //load winning scene
//        SceneManager.LoadScene("MWinScene");
    }
}
