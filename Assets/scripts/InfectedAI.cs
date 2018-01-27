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
    float hitTimer;
    public float timeToHit;
	GameObject NormieTarget;
	RaycastHit hit;
    GameObject NormieScript;
	Collider[] NormieDetected;
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
        NormieDetected = Physics.OverlapSphere (transform.position, DetectorRadius, ~(1 << LayerMask.NameToLayer ("Infected") | 1 | 1 << LayerMask.NameToLayer ("Player")));

        if (NormieDetected.Length > 0) 
        {
            anime.SetInteger("InfectedAnim",2);
            mess.mesh = chasingInfected;
            messRend.materials = new Material[2] { chasingInfectedFront, chasingInfectedBack};
            Debug.Log ("Braainsss!");
            agent.speed = chaseSpeed;
            Vector3 newPos = NormieDetected[0].transform.position;
            agent.SetDestination (newPos);
        }
        else 
        {
            mess.mesh = passiveInfected;
            messRend.materials = new Material[2] { passiveInfectedFront, passiveInfectedBack};
            timer += Time.deltaTime;
            agent.speed = normalSpeed;
            if (timer >= wanderTimer) {
                Vector3 newPos = RandomNavSphere (transform.position, wanderRadius, -1);
                agent.SetDestination (newPos);
                timer = 0;
            }
        }
        if(agent.velocity.magnitude < idleLimit)
        {
            anime.SetInteger("InfectedAnim",0);
        }
        else
            anime.SetInteger("InfectedAnim",1);
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
