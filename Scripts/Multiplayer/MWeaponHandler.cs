using UnityEngine;
using System.Collections;

//using UnityEngine.Networking;

public class MWeaponHandler : MonoBehaviour{
    [SerializeField] private GameObject[] weaponArray;
    [SerializeField] private int weaponNum = 0;
    [SerializeField] private GameObject weaponPos;
    [SerializeField] public GameObject curWeapon;
    [SerializeField] bool changeWeapon = false;

    void Awake(){
        foreach(GameObject weapon in weaponArray){
            weapon.SetActive(false);
        }
        curWeapon = weaponArray[0];
    }

    void FixedUpdate(){
//        if(!isLocalPlayer){
//            return;
//        } else{
        //Change Weapon
        if(changeWeapon){
            SwitchWeapon();
        }
        //Debug
//        if(Input.GetKeyDown(KeyCode.O)){
//            changeWeapon = true;
//        }
//
        if(Input.GetKeyDown(KeyCode.I)){
            changeWeapon = true;
            weaponNum = 0;
        }

        if(Input.GetKeyDown(KeyCode.K)){
            changeWeapon = true;
            weaponNum = 1;
        }
//        }
    }

    void SwitchWeapon(){
        changeWeapon = false;
        //Destroy(curWeapon);
        curWeapon.SetActive(false);
        curWeapon = weaponArray[weaponNum]; //= Instantiate(weaponArray[weaponNum], new Vector3(weaponPos.transform.position.x, weaponPos.transform.position.y - 1.2f, weaponPos.transform.position.z), weaponPos.transform.rotation) as GameObject;
        this.gameObject.GetComponent<MFPSCombat>().InitWeapon(curWeapon);
        curWeapon.SetActive(true);
        curWeapon.transform.parent = weaponPos.transform;
        curWeapon.transform.localPosition = new Vector3(0, -1.3f, -0.15f); 
    }
}
