using UnityEngine;
using System.Collections;

public class PrisonerAI : MonoBehaviour {

	// Use this for initialization
	private NavMeshAgent agent;
	private Vector3 dest; 
	private GameObject prisoner;

	void Start () {
		prisoner = this.gameObject;
		dest = new Vector3 (85f, 14f, 55f);
		agent = GetComponent<NavMeshAgent>();
		agent.SetDestination(dest);
		Destroy (prisoner, 70f);
	}

	void Awake(){
		//this.GetComponent<Animator>().SetBool ("WalkForward",true);
	}

	// Update is called once per frame
	void Update () {
	
	}
}
