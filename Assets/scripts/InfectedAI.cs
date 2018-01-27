using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InfectedAI : MonoBehaviour {

	public float wanderRadius;
	public float wanderTimer;
	public float DetectorRadius;
    public bool NormieNearby = false;
    public float damage;
    public float chaseSpeed;
    float normalSpeed;
    Transform target;
	NavMeshAgent agent;
	float timer;
	GameObject NormieTarget;
	RaycastHit hit;
    GameObject NormieScript;
	Collider[] NormieDetected;

	void Start () 
	{
		agent = GetComponent<NavMeshAgent> ();
        normalSpeed = agent.speed;
		timer = wanderTimer;
	}

	void Update () 
	{
        NormieDetected = Physics.OverlapSphere (transform.position, DetectorRadius, ~(1 << LayerMask.NameToLayer ("Infected") | 1 | 1 << LayerMask.NameToLayer ("Player")));

        if (NormieDetected.Length > 0) 
        {
            Debug.Log ("Braainsss!");
            agent.speed = chaseSpeed;
            Vector3 newPos = NormieDetected[0].transform.position;
            agent.SetDestination (newPos);
        }
        else 
        {
            timer += Time.deltaTime;
            agent.speed = normalSpeed;
            if (timer >= wanderTimer) {
                Vector3 newPos = RandomNavSphere (transform.position, wanderRadius, -1);
                agent.SetDestination (newPos);
                timer = 0;
            }
        }
	}


	Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask) 
	{
		Vector3 randDirection = Random.insideUnitSphere * dist;
		randDirection += origin;
		NavMeshHit navHit;
		NavMesh.SamplePosition (randDirection, out navHit, dist, layermask);
		return navHit.position;
	}

    void OnCollisionEnter (Collision other)
    {
        if (other.gameObject.tag == "Normie")
        {
            NormieAI normieAI = other.gameObject.GetComponent<NormieAI>();
            normieAI.takeDamage();
        }
    }
}
