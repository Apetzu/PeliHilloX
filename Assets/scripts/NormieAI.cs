using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NormieAI : MonoBehaviour {

    public float fleeRadius;
    public float wanderRadius;
    public float wanderTimer;
    public float DetectorRadius;
    public bool InfectedNearby = false;
    public float multiplyBy;
    public float fasterSpeed;
    public float health;

    [SerializeField]
    Transform InfectedAIPrefab;

    Transform startTransform;
	Transform target;
	NavMeshAgent agent;
	float timer;
	RaycastHit hit;
    float starthealth;

	Collider[] InfectedDetected;

	void Start () 
	{
        starthealth = health;
		agent = GetComponent<NavMeshAgent> ();
		timer = wanderTimer;
	}

	void Update () 
	{
		InfectedDetected = Physics.OverlapSphere (transform.position, DetectorRadius, ~(1 << LayerMask.NameToLayer ("Normies") | 1));

		if (InfectedDetected.Length > 0) 
		{
            Flee();
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
        Vector3 FleeDirection = -(InfectedDetected[0].transform.position - transform.position);
        transform.rotation = Quaternion.LookRotation(new Vector3(FleeDirection.x, 0, FleeDirection.z));
        Vector3 FleeTo = transform.position + transform.forward * multiplyBy;
        NavMeshHit navHitFlee;
        NavMesh.SamplePosition(FleeTo, out navHitFlee, 5, 1 << NavMesh.GetAreaFromName("Walkable"));
        agent.SetDestination(navHitFlee.position);
    }

	Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask) 
	{
		Vector3 randDirection = Random.insideUnitSphere * dist;
		randDirection += origin;
		NavMeshHit navHit;
		NavMesh.SamplePosition (randDirection, out navHit, dist, layermask);
		return navHit.position;
	}

    public void takeDamage()
    {
        health--;

        if (health <= 0)
        {
            Destroy(this.gameObject);
        }

        agent.speed = fasterSpeed;
    }

    void death()
    {
        
    }

    public void ChangeToInfected()
    {
        Instantiate(InfectedAIPrefab, transform.position, InfectedAIPrefab.transform.rotation);
        Destroy(this.gameObject);
    }
}
