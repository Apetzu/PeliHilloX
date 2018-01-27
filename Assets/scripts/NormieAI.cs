using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NormieAI : MonoBehaviour {

    private Transform startTransform;
    public float fleeRadius;
	public float wanderRadius;
	public float wanderTimer;
	public float DetectorRadius;
	private Transform target;
	private NavMeshAgent agent;
	private float timer;
	public bool InfectedNearby = false;
    public float multiplyBy;
	RaycastHit hit;

	Collider[] InfectedDetected;

	void Start () 
	{
		agent = GetComponent<NavMeshAgent> ();
		timer = wanderTimer;
	}

	void Update () 
	{
		InfectedDetected = Physics.OverlapSphere (transform.position, DetectorRadius, ~(1 << LayerMask.NameToLayer ("Normies") | 1));

		if (InfectedDetected.Length > 0) 
		{
            Flee();
            /*
            Vector3 fleeDir = -(InfectedDetected[0].transform.position + transform.position).normalized;
            //Debug.Log(InfectedDetected[0].transform.position);
            //Debug.Log((InfectedDetected[0].transform.position - transform.position).normalized);
            agent.SetDestination (fleeDir * 10);
            */
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
    void Flee()
    {
        //startTransform = transform;
        transform.rotation = Quaternion.LookRotation(transform.position - InfectedDetected[0].transform.position);
        Vector3 FleeTo = transform.position + transform.forward * multiplyBy;
        NavMeshHit navHitFlee;
        NavMesh.SamplePosition(FleeTo, out navHitFlee, 5, 1 << NavMesh.GetAreaFromName("Default"));
        agent.SetDestination(navHitFlee.position);
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
