//Will have to see if this still works on single player after making the changes

using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class DoorController1 : NetworkBehaviour{

    //[SerializeField] Transform door;
    [SerializeField] bool drawGUI = false;
    [SerializeField] bool doorIsClosed = true;
    [SerializeField] Animator anim;
    [SerializeField] bool openAndClose = true;
    [SerializeField] bool auto = false;
    [SerializeField] float openCloseDelay = 3f;
    [SerializeField] Texture eTex;
    [SyncVar(hook = "ChangeDoorState")]public bool changingState = false;

    // Use this for initialization
    void Start(){
		
    }

    void Awake(){
        if(!isLocalPlayer){
            return;
        }
        if(!doorIsClosed){ //Makes the door start as open
            doorIsClosed = true;
        }
    }
	
    // Update is called once per frame
    void FixedUpdate(){
        if(isLocalPlayer){
            if(doorIsClosed && !openAndClose){
                if((drawGUI) && (Input.GetKeyDown(KeyCode.E))){
                    doorIsClosed = !doorIsClosed;
                    changingState = doorIsClosed;
                }
            }
        }
    }

    void OnTriggerEnter(Collider col){
        //Debug.Log ("Boop");
        if(!isLocalPlayer){
            return;
        }
        if(col.tag == "Player"){
            if(auto){
                Debug.Log("Auto: Open Check");
                if(doorIsClosed){
                    Debug.Log("Auto:  is closed, open it up");
                    doorIsClosed = !doorIsClosed;
                    changingState = doorIsClosed;
                }
            } else{
                drawGUI = true;
            }
        }
    }

    void OnTriggerExit(Collider col){
        if(!isLocalPlayer){
            return;
        }
        //Debug.Log ("Beep");
        if(col.tag == "Player"){
            if(auto){
                if(!doorIsClosed){
                    doorIsClosed = !doorIsClosed;
                    changingState = doorIsClosed;
                }
            } else{
                drawGUI = false;
            }
        }
    }

    void OnGUI(){
        if(drawGUI){
            GUI.Box(new Rect(Screen.width * 0.5f - 11, Screen.height * 0.5f - 11, 22, 22), "e");
        }
    }

    void ChangeDoorState(bool changingState){
        anim.SetTrigger("ChangeState");
    }

}
