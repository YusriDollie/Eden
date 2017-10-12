using UnityEngine;
using System.Collections;

public class Billboard : MonoBehaviour{
    bool found = false;

    // Update is called once per frame
    void Update(){
        //will make the object always face the camera
        if(!found){
            if(GameObject.FindGameObjectWithTag("MainCamera")){
                found = true;
            }    
        } else{
            transform.LookAt(Camera.main.transform);
        }
    }
}