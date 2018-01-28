using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InfectedAI : MonoBehaviour {

	public float wanderRadius;
	public float wanderTimer;
	public float DetectorRadius;
    public float LongDetectorRadius;
    public bool NormieNearby = false;
    public bool NormieFarby = false;
    public float damage;
    public float chaseSpeed;
    float normalSpeed;
    Transform target;
	NavMeshAgent agent;
	float timer;
    float hitTimer;
    public float timeToHit;
	GameObject NormieTarget;
	RaycastHit hit;
    GameObject NormieScript;
	Collider[] NormieDetected;
    Collider[] NormieDetectedFar;
    public Animator anime;
    public MeshFilter mess;
    public MeshRenderer messRend;
    public Mesh passiveInfected;
    public Mesh chasingInfected;
    public Material passiveInfectedFront;
    public Material passiveInfectedBack;
    public Material chasingInfectedFront;
    public Material chasingInfectedBack;

    public float idleLimit;

	void Start () 
	{
		agent = GetComponent<NavMeshAgent> ();
        normalSpeed = agent.speed;
		timer = wanderTimer;
        hitTimer = timeToHit;
	}

	void Update () 
	{
        NormieDetected = Physics.OverlapSphere (transform.position, DetectorRadius, ~(1 << LayerMask.NameToLayer ("Infected") | 1 | 1 << LayerMask.NameToLayer ("Player") | 1 << LayerMask.NameToLayer("DisappearObject")));

        if (NormieDetected.Length > 0) 
        {
            anime.SetInteger("InfectedAnim",2);
            mess.mesh = chasingInfected;
            messRend.materials = new Material[2] { chasingInfectedFront, chasingInfectedBack};
            agent.speed = chaseSpeed;
            Vector3 newPos = NormieDetected[0].transform.position;
            agent.SetDestination (newPos);
        }
        else 
        {
            NormieDetectedFar = Physics.OverlapSphere (transform.position, LongDetectorRadius, ~(1 << LayerMask.NameToLayer ("Infected") | 1 | 1 << LayerMask.NameToLayer ("Player") | 1 << LayerMask.NameToLayer("DisappearObject")));
            if (NormieDetectedFar.Length > 0)
            {
                agent.speed = normalSpeed;
                Vector3 newPos = NormieDetectedFar[0].transform.position;
                agent.SetDestination (newPos);
            }

            if (agent.velocity.magnitude < idleLimit)
            {
                anime.SetInteger("InfectedAnim", 0);
            }
            else
                anime.SetInteger("InfectedAnim", 1);

            mess.mesh = passiveInfected;
            messRend.materials = new Material[2] { passiveInfectedFront, passiveInfectedBack};
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

    void OnCollisionStay (Collision other)
    {
        if (other.gameObject.tag == "Normie")
        {
            hitTimer += Time.deltaTime;
            if (hitTimer >= timeToHit)
            {
                NormieAI normieAI = other.gameObject.GetComponent<NormieAI>();
                normieAI.takeDamage();
                hitTimer = 0;
            }
        }
    }
}
