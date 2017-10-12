using UnityEngine;
using System.Collections;

public class VentController : MonoBehaviour{
    [SerializeField] private GameObject wholeVent;
    [SerializeField] private GameObject brokenVent;

    private BoxCollider col;

    // Use this for initialization
    void Start(){
        col = this.GetComponent<BoxCollider>();
    }
	
    // Update is called once per frame
    void Update(){
	
    }

    public void Break(){
        Debug.Log("Ouch");
        Destroy(wholeVent);
        Instantiate(brokenVent, this.transform.position, Quaternion.Euler(new Vector3(this.transform.rotation.x, this.transform.rotation.y + 90, this.transform.rotation.z)));
        col.enabled = false;
    }
}
