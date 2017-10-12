using UnityEngine;
using System.Collections;

public class ItemSpawner : MonoBehaviour {

	// Use this for initialization
	public GameObject [] Items;
	public GameObject [] Locations; 
	void Start () {
	
		foreach (GameObject obj in Locations ) {
			int randomObj =  Random.Range (0, Items.Length);
			//int randomTex=  Random.Range (0, textures.Length);
			GameObject random = (GameObject)Instantiate (Items[randomObj]);

			//Apply texture here if nessary

			//random.gameObject.GetComponent<MeshRenderer> ().material = textures [randomTex];
			random.transform.position = obj.transform.position;


		}



	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
