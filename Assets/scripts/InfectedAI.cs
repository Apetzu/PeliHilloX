using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InfectedAI : MonoBehaviour {

	public float wanderRadius;
	public float wanderTimer;
	public float DetectorRadius;
	private Transform target;
	private NavMeshAgent agent;
	private float timer;
	public bool NormieNearby = false;
	GameObject NormieTarget;
	RaycastHit hit;

	Collider[] NormieDetected;

	void Start () 
	{
		agent = GetComponent<NavMeshAgent> ();
		timer = wanderTimer;
	}

	void Update () 
	{
		NormieDetected = Physics.OverlapSphere (transform.position, DetectorRadius, ~(1 << LayerMask.NameToLayer ("Infected") | 1));

        if (NormieDetected.Length > 0) 
        {
            Debug.Log ("Braainsss!");
            Vector3 newPos = NormieDetected[0].transform.position;
            agent.SetDestination (newPos);
        }
        else 
        {
            timer += Time.deltaTime;

            if (timer >= wanderTimer) {
                Vector3 newPos = RandomNavSphere (transform.position, wanderRadius, -1);
                agent.SetDestination (newPos);
                timer = 0;
            }
        }
	}


	public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask) 
	{
		Vector3 randDirection = Random.insideUnitSphere * dist;
		randDirection += origin;
		NavMeshHit navHit;
		NavMesh.SamplePosition (randDirection, out navHit, dist, layermask);
		return navHit.position;
	}
}
