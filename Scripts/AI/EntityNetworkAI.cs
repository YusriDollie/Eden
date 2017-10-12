using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class EntityNetworkAI : NetworkBehaviour {
	private NavMeshAgent agent;
	private Transform myTransform;
	public Transform targetTransform;
	private LayerMask raycastLayer;
	private float radius = 10000;
	public GameObject[] players;
	// Use this for initialization
	void Start () {

		agent = GetComponent<NavMeshAgent>();
		myTransform = transform;
		raycastLayer = 1<<LayerMask.NameToLayer("PlayerDetCollider");
		StartCoroutine(DoCheck());
		players = GameObject.FindGameObjectsWithTag("Player");

		if(isServer)
		{
			StartCoroutine(DoCheck());
		}
	
	}
	void moveTowards(){

		Transform dest = players [0].transform;
		agent.SetDestination (dest.position);

	}


	void FixedUpdate(){


	}

	// Update is called once per frame
	void Update () {

	}


	void Searching()
	{
		if(!isServer)
		{
			return;
		}

		if(targetTransform == null)
		{
			Collider[] hitColliders = Physics.OverlapSphere(myTransform.position, radius, raycastLayer);

			if(hitColliders.Length>0)
			{
				int randomint = Random.Range(0, hitColliders.Length);
				targetTransform = hitColliders[randomint].transform;
			}
		}

		if(targetTransform != null && targetTransform.GetComponent<BoxCollider>().enabled == false)
		{
			targetTransform = null;
		}
	}


	void MoveToTarget()
	{
		if(targetTransform != null && isServer)
		{
			SetNavDestination(targetTransform);
		}
	}

	void SetNavDestination(Transform dest)
	{
		agent.SetDestination(dest.position);
	}



	IEnumerator DoCheck()
	{
		for(;;)
		{
			Searching();
			MoveToTarget();
			yield return new WaitForSeconds(0.2f);
		}
	}
}
