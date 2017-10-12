using UnityEngine;
using System.Collections;

public class DoorController : MonoBehaviour{

    //[SerializeField] Transform door;
    [SerializeField] bool drawGUI = false;
    [SerializeField] bool doorIsClosed = true;
    [SerializeField] Animator anim;
    [SerializeField] bool openAndClose = true;
    [SerializeField] bool auto = false;
    [SerializeField] float openCloseDelay = 3f;
    [SerializeField] GameObject select;
    private bool changeDisplay = true;

    // Use this for initialization
    void Start(){

    }

    void Awake(){
        if(!doorIsClosed){ //Makes the door start as open
            doorIsClosed = true;
            StartCoroutine("ChangeDoorState");
        }
    }

    // Update is called once per frame
    void Update(){
        if(openAndClose){
            if((drawGUI) && (Input.GetKeyDown(KeyCode.E))){
                StartCoroutine("ChangeDoorState");
            }
        }
    }

    void FixedUpdate(){

        if(drawGUI){
            if(changeDisplay){
                select.SetActive(true);
                changeDisplay = false;
            }
        } else{
            if(changeDisplay){
                select.SetActive(false);
                changeDisplay = false;
            }
        }
    }

    void OnTriggerEnter(Collider col){
        changeDisplay = true;
        //Debug.Log ("Boop");
        if(col.tag == "Player"){
            if(auto){
                Debug.Log("Auto: Open Check");
                if(doorIsClosed){
                    Debug.Log("Auto:  is closed, open it up");
                    ChangeDoorState();
                }
            } else{
                drawGUI = true;
            }
        }
    }

    void OnTriggerExit(Collider col){
        changeDisplay = true;
        //Debug.Log ("Beep");
        if(col.tag == "Player"){
            if(auto){
                if(!doorIsClosed){
                    ChangeDoorState();
                }
            } else{
                drawGUI = false;
            }
        }
    }

    void OnGUI(){

    }


    void ChangeDoorState(){
        anim.SetTrigger("ChangeState");
    }
}
