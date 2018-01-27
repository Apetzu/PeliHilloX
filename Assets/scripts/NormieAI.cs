﻿using System.Collections;
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

    public MeshFilter mess;
    public MeshRenderer messRend;
    public Mesh passiveHuman;
    public Mesh fleeingHuman;
    public Material passiveHumanFront;
    public Material passiveHumanBack;
    public Material fleeingHumanFront;
    public Material fleeingHumanBack;
    [SerializeField]
    Transform InfectedAIPrefab;

    Transform startTransform;
	Transform target;
	NavMeshAgent agent;
	float timer;
	RaycastHit hit;
    float starthealth;

	Collider[] InfectedDetected;
    public Animator anime;
    public float idleLimit;

	void Start () 
	{
        
        starthealth = health;
        wanderTimer = Random.Range(1.5f,3f);
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
            mess.mesh = passiveHuman;
            messRend.materials = new Material[2] { passiveHumanFront, passiveHumanBack};
            timer += Time.deltaTime;

            if (timer >= wanderTimer) {
                Vector3 newPos = RandomNavSphere (transform.position, wanderRadius, -1);
                agent.SetDestination (newPos);
                timer = 0;
            }
		}
        if(agent.velocity.magnitude < idleLimit)
        {
            anime.SetInteger("AnimPos",0);
        }
        else
            anime.SetInteger("AnimPos",1);
	}

    void Flee()
    {
        anime.SetInteger("AnimPos",2);
        mess.mesh = fleeingHuman;
        messRend.materials = new Material[2] { fleeingHumanFront, fleeingHumanBack};
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
