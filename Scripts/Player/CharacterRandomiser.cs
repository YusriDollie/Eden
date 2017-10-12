using UnityEngine;
using System.Collections;

public class CharacterRandomiser : MonoBehaviour {
	[SerializeField] private Color[] skinColour;
	[SerializeField] private Color[] shirtColour;
	[SerializeField] private Color[] sleaveColour;

	/*[SerializeField] private Material skin;
	[SerializeField] private Material shirt;
	[SerializeField] private Material sleaves;
	[SerializeField] private Material pants;*/

	[SerializeField] private Renderer ren;

	// Use this for initialization
	void Start () {
	
	}

	void Awake(){

		ren.materials[1].color = skinColour [Random.Range (0, skinColour.Length)];

		int randomShirt = Random.Range (0, shirtColour.Length);
		if (randomShirt == 0) { //Bare chested...ie no sleaves
			ren.materials [0].color = ren.materials [1].color;	//shirt
			ren.materials [3].color = ren.materials [1].color;	//Sleaves
		} else if (randomShirt == 1) {	//Overalls
			//Debug.Log ("Overalls");
			ren.materials[0].color = shirtColour [1];
			ren.materials [3].color = sleaveColour [1];
		} else { //wife beater...bare arms
			ren.materials[0].color = shirtColour [2];
			ren.materials [3].color = ren.materials [1].color;
		}

		//ren.material.Human_Skin

		/*skin.color = skinColour [Random.Range (0, skinColour.Length)];

		int randomShirt = Random.Range (0, shirtColour.Length);
		if (randomShirt == 0) { //Bare chested...ie no sleaves
			shirt.color = skin.color;
			sleaves.color = skin.color;
		} else {
			shirt.color = shirtColour [randomShirt];

			if (randomShirt == 2) { //2 = white vest...ie no orange arms
				sleaves.color = skin.color;
			} else {
				sleaves.color = shirt.color;
			}
		}*/
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
