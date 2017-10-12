using UnityEngine;
using System.Collections;

public class BossEvent : MonoBehaviour{
    public GameObject enemies;

    void OnTriggerEnter(Collider col){
        if(col.tag == "Player"){
            enemies.SetActive(false);
            if(col.GetComponent<AudioManager>().currentTrack != 3){
                col.GetComponent<AudioManager>().PlayTrack(3);

            }
        }
    }
}
