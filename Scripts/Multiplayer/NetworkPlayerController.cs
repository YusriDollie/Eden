using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityStandardAssets.Characters.FirstPerson;

public class NetworkPlayerController : NetworkBehaviour{
    [SerializeField] public GameObject weaponCam;
    [SerializeField] public GameObject worldGameObject;
    [SerializeField] public float health = 0;
    [SerializeField] public int stamina = 0;
    [SerializeField] public bool attack = false;
    [SerializeField] public int attackComboPos = 0;
    [SerializeField] public int weaponNum = 0;
    [SerializeField] public int playerID;
    public GameObject healthbar;
    public GameObject staminabar;

    string remoteLayer = "RemotePlayer";

     
    // Use this for initialization
    void Start(){
        if(isLocalPlayer){
            weaponCam.SetActive(true);
            worldGameObject.SetActive(false);
        } else{
            gameObject.layer = LayerMask.NameToLayer(remoteLayer);
            weaponCam.SetActive(false);
            worldGameObject.SetActive(true);
//            GetComponent<CharacterController>().enabled = false;
            GetComponent<MFirstPersonController>().enabled = false;

        }
//        this.transform.position = new Vector3(156.0f, 32.0f, -108.0f);

    }

    void FixedUpdate(){
        if(worldGameObject.activeSelf){//may need to changed for relative positioning
            worldGameObject.transform.position = transform.position;
            worldGameObject.transform.rotation = transform.rotation;//updown rotation error            
        }
    }

    public override void OnStartClient(){
        base.OnStartClient();

        int netID = (int)GetComponent<NetworkIdentity>().netId.Value;
        playerID = netID - 1;

        GameManager.RegisterPlayer(netID, GetComponent<NetworkPlayerController>());
    }
}
