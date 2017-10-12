using UnityEngine;
using System.Collections;

public class MeleeWeapon : MonoBehaviour{

    [SerializeField] public float ROFDeadzone = 0f;
    //Dellay such that players can "combo"
    [SerializeField] public GameObject arms;

    [SerializeField] public int[] combo;
    [SerializeField] public float[] comboDist;
    [SerializeField] public float[] comboDelay;
    [SerializeField] public float[] comboDamage;

    [SerializeField] private GameObject[] weaponEdge;

    public string weaponName = "";
    //Add image for UI?

    // Use this for initialization
    void Start(){
		
    }

    void Awake(){
        //weaponEdge = gameObject.GetComponentsInChildren<BoxCollider> ();
        //ROFDeadzone = ROFDeadzone * 1000;
    }
	
    // Update is called once per frame
    void Update(){
	
    }
}
